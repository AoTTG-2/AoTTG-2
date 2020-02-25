using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Material material;
    [HideInInspector, SerializeField]
    private Coordinates mCoordinates;
    [HideInInspector, SerializeField]
    private float mPixelSize = 1f;
    private int mPMA = -1;
    [SerializeField, HideInInspector]
    private UIAtlas mReplacement;
    [SerializeField, HideInInspector]
    private List<Sprite> sprites = new List<Sprite>();

    public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
    {
        return (((a != null) && (b != null)) && (((a == b) || a.References(b)) || b.References(a)));
    }

    private static int CompareString(string a, string b)
    {
        return a.CompareTo(b);
    }

    public BetterList<string> GetListOfSprites()
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetListOfSprites();
        }
        BetterList<string> list = new BetterList<string>();
        int num = 0;
        int count = this.sprites.Count;
        while (num < count)
        {
            Sprite sprite = this.sprites[num];
            if ((sprite != null) && !string.IsNullOrEmpty(sprite.name))
            {
                list.Add(sprite.name);
            }
            num++;
        }
        return list;
    }

    public BetterList<string> GetListOfSprites(string match)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetListOfSprites(match);
        }
        if (string.IsNullOrEmpty(match))
        {
            return this.GetListOfSprites();
        }
        BetterList<string> list = new BetterList<string>();
        int num = 0;
        int count = this.sprites.Count;
        while (num < count)
        {
            Sprite sprite = this.sprites[num];
            if (((sprite != null) && !string.IsNullOrEmpty(sprite.name)) && string.Equals(match, sprite.name, StringComparison.OrdinalIgnoreCase))
            {
                list.Add(sprite.name);
                return list;
            }
            num++;
        }
        char[] separator = new char[] { ' ' };
        string[] strArray = match.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strArray.Length; i++)
        {
            strArray[i] = strArray[i].ToLower();
        }
        int num4 = 0;
        int num5 = this.sprites.Count;
        while (num4 < num5)
        {
            Sprite sprite2 = this.sprites[num4];
            if ((sprite2 != null) && !string.IsNullOrEmpty(sprite2.name))
            {
                string str = sprite2.name.ToLower();
                int num6 = 0;
                for (int j = 0; j < strArray.Length; j++)
                {
                    if (str.Contains(strArray[j]))
                    {
                        num6++;
                    }
                }
                if (num6 == strArray.Length)
                {
                    list.Add(sprite2.name);
                }
            }
            num4++;
        }
        return list;
    }

    public Sprite GetSprite(string name)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetSprite(name);
        }
        if (!string.IsNullOrEmpty(name))
        {
            int num = 0;
            int count = this.sprites.Count;
            while (num < count)
            {
                Sprite sprite = this.sprites[num];
                if (!string.IsNullOrEmpty(sprite.name) && (name == sprite.name))
                {
                    return sprite;
                }
                num++;
            }
        }
        return null;
    }

    public void MarkAsDirty()
    {
        if (this.mReplacement != null)
        {
            this.mReplacement.MarkAsDirty();
        }
        UISprite[] spriteArray = NGUITools.FindActive<UISprite>();
        int index = 0;
        int length = spriteArray.Length;
        while (index < length)
        {
            UISprite sprite = spriteArray[index];
            if (CheckIfRelated(this, sprite.atlas))
            {
                UIAtlas atlas = sprite.atlas;
                sprite.atlas = null;
                sprite.atlas = atlas;
            }
            index++;
        }
        UIFont[] fontArray = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
        int num3 = 0;
        int num4 = fontArray.Length;
        while (num3 < num4)
        {
            UIFont font = fontArray[num3];
            if (CheckIfRelated(this, font.atlas))
            {
                UIAtlas atlas2 = font.atlas;
                font.atlas = null;
                font.atlas = atlas2;
            }
            num3++;
        }
        UILabel[] labelArray = NGUITools.FindActive<UILabel>();
        int num5 = 0;
        int num6 = labelArray.Length;
        while (num5 < num6)
        {
            UILabel label = labelArray[num5];
            if ((label.font != null) && CheckIfRelated(this, label.font.atlas))
            {
                UIFont font2 = label.font;
                label.font = null;
                label.font = font2;
            }
            num5++;
        }
    }

    private bool References(UIAtlas atlas)
    {
        if (atlas == null)
        {
            return false;
        }
        return ((atlas == this) || ((this.mReplacement != null) && this.mReplacement.References(atlas)));
    }

    public Coordinates coordinates
    {
        get
        {
            return ((this.mReplacement == null) ? this.mCoordinates : this.mReplacement.coordinates);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.coordinates = value;
            }
            else if (this.mCoordinates != value)
            {
                if ((this.material == null) || (this.material.mainTexture == null))
                {
                    Debug.LogError("Can't switch coordinates until the atlas material has a valid texture");
                }
                else
                {
                    this.mCoordinates = value;
                    Texture mainTexture = this.material.mainTexture;
                    int num = 0;
                    int count = this.sprites.Count;
                    while (num < count)
                    {
                        Sprite sprite = this.sprites[num];
                        if (this.mCoordinates == Coordinates.TexCoords)
                        {
                            sprite.outer = NGUIMath.ConvertToTexCoords(sprite.outer, mainTexture.width, mainTexture.height);
                            sprite.inner = NGUIMath.ConvertToTexCoords(sprite.inner, mainTexture.width, mainTexture.height);
                        }
                        else
                        {
                            sprite.outer = NGUIMath.ConvertToPixels(sprite.outer, mainTexture.width, mainTexture.height, true);
                            sprite.inner = NGUIMath.ConvertToPixels(sprite.inner, mainTexture.width, mainTexture.height, true);
                        }
                        num++;
                    }
                }
            }
        }
    }

    public float pixelSize
    {
        get
        {
            return ((this.mReplacement == null) ? this.mPixelSize : this.mReplacement.pixelSize);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.pixelSize = value;
            }
            else
            {
                float num = Mathf.Clamp(value, 0.25f, 4f);
                if (this.mPixelSize != num)
                {
                    this.mPixelSize = num;
                    this.MarkAsDirty();
                }
            }
        }
    }

    public bool premultipliedAlpha
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.premultipliedAlpha;
            }
            if (this.mPMA == -1)
            {
                Material spriteMaterial = this.spriteMaterial;
                this.mPMA = (((spriteMaterial == null) || (spriteMaterial.shader == null)) || !spriteMaterial.shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public UIAtlas replacement
    {
        get
        {
            return this.mReplacement;
        }
        set
        {
            UIAtlas atlas = value;
            if (atlas == this)
            {
                atlas = null;
            }
            if (this.mReplacement != atlas)
            {
                if ((atlas != null) && (atlas.replacement == this))
                {
                    atlas.replacement = null;
                }
                if (this.mReplacement != null)
                {
                    this.MarkAsDirty();
                }
                this.mReplacement = atlas;
                this.MarkAsDirty();
            }
        }
    }

    public List<Sprite> spriteList
    {
        get
        {
            return ((this.mReplacement == null) ? this.sprites : this.mReplacement.spriteList);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.spriteList = value;
            }
            else
            {
                this.sprites = value;
            }
        }
    }

    public Material spriteMaterial
    {
        get
        {
            return ((this.mReplacement == null) ? this.material : this.mReplacement.spriteMaterial);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.spriteMaterial = value;
            }
            else if (this.material == null)
            {
                this.mPMA = 0;
                this.material = value;
            }
            else
            {
                this.MarkAsDirty();
                this.mPMA = -1;
                this.material = value;
                this.MarkAsDirty();
            }
        }
    }

    public Texture texture
    {
        get
        {
            return ((this.mReplacement == null) ? ((this.material == null) ? null : this.material.mainTexture) : this.mReplacement.texture);
        }
    }

    public enum Coordinates
    {
        Pixels,
        TexCoords
    }

    [Serializable]
    public class Sprite
    {
        public Rect inner = new Rect(0f, 0f, 1f, 1f);
        public string name = "Unity Bug";
        public Rect outer = new Rect(0f, 0f, 1f, 1f);
        public float paddingBottom;
        public float paddingLeft;
        public float paddingRight;
        public float paddingTop;
        public bool rotated;

        public bool hasPadding
        {
            get
            {
                return ((((this.paddingLeft != 0f) || (this.paddingRight != 0f)) || (this.paddingTop != 0f)) || (this.paddingBottom != 0f));
            }
        }
    }
}

