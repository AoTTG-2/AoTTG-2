using AnimationOrTween;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class UITweener : IgnoreTimeScale
{
    public AnimationCurve animationCurve;
    public string callWhenFinished;
    public float delay;
    public float duration;
    public GameObject eventReceiver;
    public bool ignoreTimeScale;
    private float mAmountPerDelta;
    private float mDuration;
    public Method method;
    private float mFactor;
    private bool mStarted;
    private float mStartTime;
    public OnFinished onFinished;
    public bool steeperCurves;
    public Style style;
    public int tweenGroup;

    protected UITweener()
    {
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) };
        this.animationCurve = new AnimationCurve(keys);
        this.ignoreTimeScale = true;
        this.duration = 1f;
        this.mAmountPerDelta = 1f;
    }

    public static T Begin<T>(GameObject go, float duration) where T: UITweener
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        component.mStarted = false;
        component.duration = duration;
        component.mFactor = 0f;
        component.mAmountPerDelta = Mathf.Abs(component.mAmountPerDelta);
        component.style = Style.Once;
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) };
        component.animationCurve = new AnimationCurve(keys);
        component.eventReceiver = null;
        component.callWhenFinished = null;
        component.onFinished = null;
        component.enabled = true;
        return component;
    }

    private float BounceLogic(float val)
    {
        if (val < 0.363636f)
        {
            val = (7.5685f * val) * val;
            return val;
        }
        if (val < 0.727272f)
        {
            val = ((7.5625f * (val -= 0.545454f)) * val) + 0.75f;
            return val;
        }
        if (val < 0.90909f)
        {
            val = ((7.5625f * (val -= 0.818181f)) * val) + 0.9375f;
            return val;
        }
        val = ((7.5625f * (val -= 0.9545454f)) * val) + 0.984375f;
        return val;
    }

    private void OnDisable()
    {
        this.mStarted = false;
    }

    protected abstract void OnUpdate(float factor, bool isFinished);
    public void Play(bool forward)
    {
        this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        if (!forward)
        {
            this.mAmountPerDelta = -this.mAmountPerDelta;
        }
        base.enabled = true;
    }

    public void Reset()
    {
        this.mStarted = false;
        this.mFactor = (this.mAmountPerDelta >= 0f) ? 0f : 1f;
        this.Sample(this.mFactor, false);
    }

    public void Sample(float factor, bool isFinished)
    {
        float f = Mathf.Clamp01(factor);
        if (this.method == Method.EaseIn)
        {
            f = 1f - Mathf.Sin(1.570796f * (1f - f));
            if (this.steeperCurves)
            {
                f *= f;
            }
        }
        else if (this.method == Method.EaseOut)
        {
            f = Mathf.Sin(1.570796f * f);
            if (this.steeperCurves)
            {
                f = 1f - f;
                f = 1f - (f * f);
            }
        }
        else if (this.method == Method.EaseInOut)
        {
            f -= Mathf.Sin(f * 6.283185f) / 6.283185f;
            if (this.steeperCurves)
            {
                f = (f * 2f) - 1f;
                float num2 = Mathf.Sign(f);
                f = 1f - Mathf.Abs(f);
                f = 1f - (f * f);
                f = ((num2 * f) * 0.5f) + 0.5f;
            }
        }
        else if (this.method == Method.BounceIn)
        {
            f = this.BounceLogic(f);
        }
        else if (this.method == Method.BounceOut)
        {
            f = 1f - this.BounceLogic(1f - f);
        }
        this.OnUpdate((this.animationCurve == null) ? f : this.animationCurve.Evaluate(f), isFinished);
    }

    private void Start()
    {
        this.Update();
    }

    public void Toggle()
    {
        if (this.mFactor > 0f)
        {
            this.mAmountPerDelta = -this.amountPerDelta;
        }
        else
        {
            this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        }
        base.enabled = true;
    }

    private void Update()
    {
        float num = !this.ignoreTimeScale ? Time.deltaTime : base.UpdateRealTimeDelta();
        float num2 = !this.ignoreTimeScale ? Time.time : base.realTime;
        if (!this.mStarted)
        {
            this.mStarted = true;
            this.mStartTime = num2 + this.delay;
        }
        if (num2 >= this.mStartTime)
        {
            this.mFactor += this.amountPerDelta * num;
            if (this.style == Style.Loop)
            {
                if (this.mFactor > 1f)
                {
                    this.mFactor -= Mathf.Floor(this.mFactor);
                }
            }
            else if (this.style == Style.PingPong)
            {
                if (this.mFactor > 1f)
                {
                    this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
                    this.mAmountPerDelta = -this.mAmountPerDelta;
                }
                else if (this.mFactor < 0f)
                {
                    this.mFactor = -this.mFactor;
                    this.mFactor -= Mathf.Floor(this.mFactor);
                    this.mAmountPerDelta = -this.mAmountPerDelta;
                }
            }
            if ((this.style == Style.Once) && ((this.mFactor > 1f) || (this.mFactor < 0f)))
            {
                this.mFactor = Mathf.Clamp01(this.mFactor);
                this.Sample(this.mFactor, true);
                if (this.onFinished != null)
                {
                    this.onFinished(this);
                }
                if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                {
                    this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
                }
                if (((this.mFactor == 1f) && (this.mAmountPerDelta > 0f)) || ((this.mFactor == 0f) && (this.mAmountPerDelta < 0f)))
                {
                    base.enabled = false;
                }
            }
            else
            {
                this.Sample(this.mFactor, false);
            }
        }
    }

    public float amountPerDelta
    {
        get
        {
            if (this.mDuration != this.duration)
            {
                this.mDuration = this.duration;
                this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration));
            }
            return this.mAmountPerDelta;
        }
    }

    public AnimationOrTween.Direction direction
    {
        get
        {
            return ((this.mAmountPerDelta >= 0f) ? AnimationOrTween.Direction.Forward : AnimationOrTween.Direction.Reverse);
        }
    }

    public float tweenFactor
    {
        get
        {
            return this.mFactor;
        }
    }

    public enum Method
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        BounceIn,
        BounceOut
    }

    public delegate void OnFinished(UITweener tween);

    public enum Style
    {
        Once,
        Loop,
        PingPong
    }
}

