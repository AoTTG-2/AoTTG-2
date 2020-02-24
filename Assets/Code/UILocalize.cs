using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget)), AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
    public string key;
    private string mLanguage;
    private bool mStarted;

    public void Localize()
    {
        Localization instance = Localization.instance;
        UIWidget component = base.GetComponent<UIWidget>();
        UILabel label = component as UILabel;
        UISprite sprite = component as UISprite;
        if ((string.IsNullOrEmpty(this.mLanguage) && string.IsNullOrEmpty(this.key)) && (label != null))
        {
            this.key = label.text;
        }
        string str = !string.IsNullOrEmpty(this.key) ? instance.Get(this.key) : string.Empty;
        if (label != null)
        {
            UIInput input = NGUITools.FindInParents<UIInput>(label.gameObject);
            if ((input != null) && (input.label == label))
            {
                input.defaultText = str;
            }
            else
            {
                label.text = str;
            }
        }
        else if (sprite != null)
        {
            sprite.spriteName = str;
            sprite.MakePixelPerfect();
        }
        this.mLanguage = instance.currentLanguage;
    }

    private void OnEnable()
    {
        if (this.mStarted && (Localization.instance != null))
        {
            this.Localize();
        }
    }

    private void OnLocalize(Localization loc)
    {
        if (this.mLanguage != loc.currentLanguage)
        {
            this.Localize();
        }
    }

    private void Start()
    {
        this.mStarted = true;
        if (Localization.instance != null)
        {
            this.Localize();
        }
    }
}

