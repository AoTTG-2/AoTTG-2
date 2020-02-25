using AnimationOrTween;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Animation)), AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : IgnoreTimeScale
{
    public string callWhenFinished;
    public GameObject eventReceiver;
    private Animation mAnim;
    private AnimationOrTween.Direction mDisableDirection;
    private AnimationOrTween.Direction mLastDirection;
    private bool mNotify;
    public OnFinished onFinished;

    private void Play(string clipName, AnimationOrTween.Direction playDirection)
    {
        if (this.mAnim != null)
        {
            base.enabled = true;
            this.mAnim.enabled = false;
            if (playDirection == AnimationOrTween.Direction.Toggle)
            {
                playDirection = (this.mLastDirection == AnimationOrTween.Direction.Forward) ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward;
            }
            if (string.IsNullOrEmpty(clipName))
            {
                if (!this.mAnim.isPlaying)
                {
                    this.mAnim.Play();
                }
            }
            else if (!this.mAnim.IsPlaying(clipName))
            {
                this.mAnim.Play(clipName);
            }
            IEnumerator enumerator = this.mAnim.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    if (string.IsNullOrEmpty(clipName) || (current.name == clipName))
                    {
                        float num = Mathf.Abs(current.speed);
                        current.speed = num * ((float) playDirection);
                        if ((playDirection == AnimationOrTween.Direction.Reverse) && (current.time == 0f))
                        {
                            current.time = current.length;
                        }
                        else if ((playDirection == AnimationOrTween.Direction.Forward) && (current.time == current.length))
                        {
                            current.time = 0f;
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            this.mLastDirection = playDirection;
            this.mNotify = true;
            this.mAnim.Sample();
        }
    }

    public static ActiveAnimation Play(Animation anim, AnimationOrTween.Direction playDirection)
    {
        return Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
    }

    public static ActiveAnimation Play(Animation anim, string clipName, AnimationOrTween.Direction playDirection)
    {
        return Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
    }

    public static ActiveAnimation Play(Animation anim, string clipName, AnimationOrTween.Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
    {
        if (!NGUITools.GetActive(anim.gameObject))
        {
            if (enableBeforePlay != EnableCondition.EnableThenPlay)
            {
                return null;
            }
            NGUITools.SetActive(anim.gameObject, true);
            UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                componentsInChildren[index].Refresh();
                index++;
            }
        }
        ActiveAnimation component = anim.GetComponent<ActiveAnimation>();
        if (component == null)
        {
            component = anim.gameObject.AddComponent<ActiveAnimation>();
        }
        component.mAnim = anim;
        component.mDisableDirection = (AnimationOrTween.Direction) disableCondition;
        component.eventReceiver = null;
        component.callWhenFinished = null;
        component.onFinished = null;
        component.Play(clipName, playDirection);
        return component;
    }

    public void Reset()
    {
        if (this.mAnim != null)
        {
            IEnumerator enumerator = this.mAnim.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    if (this.mLastDirection == AnimationOrTween.Direction.Reverse)
                    {
                        current.time = current.length;
                    }
                    else if (this.mLastDirection == AnimationOrTween.Direction.Forward)
                    {
                        current.time = 0f;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }

    private void Update()
    {
        float num = base.UpdateRealTimeDelta();
        if (num != 0f)
        {
            if (this.mAnim != null)
            {
                bool flag = false;
                IEnumerator enumerator = this.mAnim.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AnimationState current = (AnimationState) enumerator.Current;
                        if (this.mAnim.IsPlaying(current.name))
                        {
                            float num2 = current.speed * num;
                            current.time += num2;
                            if (num2 < 0f)
                            {
                                if (current.time > 0f)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    current.time = 0f;
                                }
                            }
                            else if (current.time < current.length)
                            {
                                flag = true;
                            }
                            else
                            {
                                current.time = current.length;
                            }
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                this.mAnim.Sample();
                if (!flag)
                {
                    base.enabled = false;
                    if (this.mNotify)
                    {
                        this.mNotify = false;
                        if (this.onFinished != null)
                        {
                            this.onFinished(this);
                        }
                        if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                        {
                            this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
                        }
                        if ((this.mDisableDirection != AnimationOrTween.Direction.Toggle) && (this.mLastDirection == this.mDisableDirection))
                        {
                            NGUITools.SetActive(base.gameObject, false);
                        }
                    }
                }
            }
            else
            {
                base.enabled = false;
            }
        }
    }

    public bool isPlaying
    {
        get
        {
            if (this.mAnim != null)
            {
                IEnumerator enumerator = this.mAnim.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AnimationState current = (AnimationState) enumerator.Current;
                        if (this.mAnim.IsPlaying(current.name))
                        {
                            if (this.mLastDirection == AnimationOrTween.Direction.Forward)
                            {
                                if (current.time < current.length)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (this.mLastDirection != AnimationOrTween.Direction.Reverse)
                                {
                                    return true;
                                }
                                if (current.time > 0f)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            return false;
        }
    }

    public delegate void OnFinished(ActiveAnimation anim);
}

