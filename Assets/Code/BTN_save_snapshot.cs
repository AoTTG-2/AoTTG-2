using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BTN_save_snapshot : MonoBehaviour
{
    public GameObject info;
    public GameObject targetTexture;
    public GameObject[] thingsNeedToHide;

    private void OnClick()
    {
        foreach (GameObject obj2 in this.thingsNeedToHide)
        {
            Transform transform = obj2.transform;
            transform.position += (Vector3) (Vector3.up * 10000f);
        }
        base.StartCoroutine(this.ScreenshotEncode());
        this.info.GetComponent<UILabel>().text = "trying..";
    }

    [DebuggerHidden]
    private IEnumerator ScreenshotEncode()
    {
        return new ScreenshotEncodec__Iterator0 { f__this = this };
    }

    [CompilerGenerated]
    private sealed class ScreenshotEncodec__Iterator0 : IEnumerator<object>, IEnumerator, IDisposable
    {
        internal object Scurrent;
        internal int SPC;
        internal GameObject[] Ss_5__2;
        internal int Ss_6__3;
        internal BTN_save_snapshot f__this;
        internal GameObject go__4;
        internal string img_name__5;
        internal float r__0;
        internal Texture2D texture__1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.Scurrent = new WaitForEndOfFrame();
                    this.SPC = 1;
                    break;

                case 1:
                    this.r__0 = ((float) Screen.height) / 600f;
                    this.texture__1 = new Texture2D((int) (this.r__0 * this.f__this.targetTexture.transform.localScale.x), (int) (this.r__0 * this.f__this.targetTexture.transform.localScale.y), TextureFormat.RGB24, false);
                    this.texture__1.ReadPixels(new Rect((Screen.width * 0.5f) - (this.texture__1.width * 0.5f), ((Screen.height * 0.5f) - (this.texture__1.height * 0.5f)) - (this.r__0 * 0f), (float) this.texture__1.width, (float) this.texture__1.height), 0, 0);
                    this.texture__1.Apply();
                    this.Scurrent = 0;
                    this.SPC = 2;
                    break;

                case 2:
                {
                    this.Ss_5__2 = this.f__this.thingsNeedToHide;
                    this.Ss_6__3 = 0;
                    while (this.Ss_6__3 < this.Ss_5__2.Length)
                    {
                        this.go__4 = this.Ss_5__2[this.Ss_6__3];
                        Transform transform = this.go__4.transform;
                        transform.position -= (Vector3) (Vector3.up * 10000f);
                        this.Ss_6__3++;
                    }
                    string[] textArray1 = new string[] { "aottg_ss-", DateTime.Today.Month.ToString(), "_", DateTime.Today.Day.ToString(), "_", DateTime.Today.Year.ToString(), "-", DateTime.Now.Hour.ToString(), "_", DateTime.Now.Minute.ToString(), "_", DateTime.Now.Second.ToString(), ".png" };
                    this.img_name__5 = string.Concat(textArray1);
                    object[] args = new object[] { this.img_name__5, this.texture__1.width, this.texture__1.height, Convert.ToBase64String(this.texture__1.EncodeToPNG()) };
                    Application.ExternalCall("SaveImg", args);
                    UnityEngine.Object.DestroyObject(this.texture__1);
                    this.SPC = -1;
                    goto Label_0300;
                }
                default:
                    goto Label_0300;
            }
            return true;
        Label_0300:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }
}

