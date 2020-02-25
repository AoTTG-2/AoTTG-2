using System;
using UnityEngine;

[RequireComponent(typeof(UISprite)), AddComponentMenu("NGUI/Examples/UI Cursor")]
public class UICursor : MonoBehaviour
{
    private UIAtlas mAtlas;
    private static UICursor mInstance;
    private UISprite mSprite;
    private string mSpriteName;
    private Transform mTrans;
    public Camera uiCamera;

    private void Awake()
    {
        mInstance = this;
    }

    public static void Clear()
    {
        Set(mInstance.mAtlas, mInstance.mSpriteName);
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    public static void Set(UIAtlas atlas, string sprite)
    {
        if (mInstance != null)
        {
            mInstance.mSprite.atlas = atlas;
            mInstance.mSprite.spriteName = sprite;
            mInstance.mSprite.MakePixelPerfect();
            mInstance.Update();
        }
    }

    private void Start()
    {
        this.mTrans = base.transform;
        this.mSprite = base.GetComponentInChildren<UISprite>();
        this.mAtlas = this.mSprite.atlas;
        this.mSpriteName = this.mSprite.spriteName;
        this.mSprite.depth = 100;
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
    }

    private void Update()
    {
        if (this.mSprite.atlas != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (this.uiCamera != null)
            {
                mousePosition.x = Mathf.Clamp01(mousePosition.x / ((float) Screen.width));
                mousePosition.y = Mathf.Clamp01(mousePosition.y / ((float) Screen.height));
                this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
                if (this.uiCamera.orthographic)
                {
                    this.mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(this.mTrans.localPosition, this.mTrans.localScale);
                }
            }
            else
            {
                mousePosition.x -= Screen.width * 0.5f;
                mousePosition.y -= Screen.height * 0.5f;
                this.mTrans.localPosition = NGUIMath.ApplyHalfPixelOffset(mousePosition, this.mTrans.localScale);
            }
        }
    }
}

