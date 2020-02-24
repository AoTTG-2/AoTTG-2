using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite Animation"), RequireComponent(typeof(UISprite)), ExecuteInEditMode]
public class UISpriteAnimation : MonoBehaviour
{
    private bool mActive = true;
    private float mDelta;
    [SerializeField, HideInInspector]
    private int mFPS = 30;
    private int mIndex;
    [SerializeField, HideInInspector]
    private bool mLoop = true;
    [SerializeField, HideInInspector]
    private string mPrefix = string.Empty;
    private UISprite mSprite;
    private List<string> mSpriteNames = new List<string>();

    private void RebuildSpriteList()
    {
        if (this.mSprite == null)
        {
            this.mSprite = base.GetComponent<UISprite>();
        }
        this.mSpriteNames.Clear();
        if ((this.mSprite != null) && (this.mSprite.atlas != null))
        {
            List<UIAtlas.Sprite> spriteList = this.mSprite.atlas.spriteList;
            int num = 0;
            int count = spriteList.Count;
            while (num < count)
            {
                UIAtlas.Sprite sprite = spriteList[num];
                if (string.IsNullOrEmpty(this.mPrefix) || sprite.name.StartsWith(this.mPrefix))
                {
                    this.mSpriteNames.Add(sprite.name);
                }
                num++;
            }
            this.mSpriteNames.Sort();
        }
    }

    public void Reset()
    {
        this.mActive = true;
        this.mIndex = 0;
        if ((this.mSprite != null) && (this.mSpriteNames.Count > 0))
        {
            this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
            this.mSprite.MakePixelPerfect();
        }
    }

    private void Start()
    {
        this.RebuildSpriteList();
    }

    private void Update()
    {
        if ((this.mActive && (this.mSpriteNames.Count > 1)) && (Application.isPlaying && (this.mFPS > 0f)))
        {
            this.mDelta += Time.deltaTime;
            float num = 1f / ((float) this.mFPS);
            if (num < this.mDelta)
            {
                this.mDelta = (num <= 0f) ? 0f : (this.mDelta - num);
                if (++this.mIndex >= this.mSpriteNames.Count)
                {
                    this.mIndex = 0;
                    this.mActive = this.loop;
                }
                if (this.mActive)
                {
                    this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
                    this.mSprite.MakePixelPerfect();
                }
            }
        }
    }

    public int frames
    {
        get
        {
            return this.mSpriteNames.Count;
        }
    }

    public int framesPerSecond
    {
        get
        {
            return this.mFPS;
        }
        set
        {
            this.mFPS = value;
        }
    }

    public bool isPlaying
    {
        get
        {
            return this.mActive;
        }
    }

    public bool loop
    {
        get
        {
            return this.mLoop;
        }
        set
        {
            this.mLoop = value;
        }
    }

    public string namePrefix
    {
        get
        {
            return this.mPrefix;
        }
        set
        {
            if (this.mPrefix != value)
            {
                this.mPrefix = value;
                this.RebuildSpriteList();
            }
        }
    }
}

