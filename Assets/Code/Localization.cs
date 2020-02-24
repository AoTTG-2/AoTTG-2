using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
    public TextAsset[] languages;
    private Dictionary<string, string> mDictionary = new Dictionary<string, string>();
    private static Localization mInstance;
    private string mLanguage;
    public string startingLanguage = "English";

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            this.currentLanguage = PlayerPrefs.GetString("Language", this.startingLanguage);
            if ((string.IsNullOrEmpty(this.mLanguage) && (this.languages != null)) && (this.languages.Length > 0))
            {
                this.currentLanguage = this.languages[0].name;
            }
        }
        else
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public string Get(string key)
    {
        string str;
        return (!this.mDictionary.TryGetValue(key, out str) ? key : str);
    }

    private void Load(TextAsset asset)
    {
        this.mLanguage = asset.name;
        PlayerPrefs.SetString("Language", this.mLanguage);
        this.mDictionary = new ByteReader(asset).ReadDictionary();
        UIRoot.Broadcast("OnLocalize", this);
    }

    public static string Localize(string key)
    {
        return ((instance == null) ? key : instance.Get(key));
    }

    private void OnDestroy()
    {
        if (mInstance == this)
        {
            mInstance = null;
        }
    }

    private void OnEnable()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }

    public string currentLanguage
    {
        get
        {
            return this.mLanguage;
        }
        set
        {
            if (this.mLanguage != value)
            {
                this.startingLanguage = value;
                if (!string.IsNullOrEmpty(value))
                {
                    if (this.languages != null)
                    {
                        int index = 0;
                        int length = this.languages.Length;
                        while (index < length)
                        {
                            TextAsset asset = this.languages[index];
                            if ((asset != null) && (asset.name == value))
                            {
                                this.Load(asset);
                                return;
                            }
                            index++;
                        }
                    }
                    TextAsset asset2 = Resources.Load(value, typeof(TextAsset)) as TextAsset;
                    if (asset2 != null)
                    {
                        this.Load(asset2);
                        return;
                    }
                }
                this.mDictionary.Clear();
                PlayerPrefs.DeleteKey("Language");
            }
        }
    }

    public static Localization instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = UnityEngine.Object.FindObjectOfType(typeof(Localization)) as Localization;
                if (mInstance == null)
                {
                    GameObject target = new GameObject("_Localization");
                    UnityEngine.Object.DontDestroyOnLoad(target);
                    mInstance = target.AddComponent<Localization>();
                }
            }
            return mInstance;
        }
    }

    public static bool isActive
    {
        get
        {
            return (mInstance != null);
        }
    }
}

