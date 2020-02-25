using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
    public string keyName;
    private UICheckbox mCheck;
    private UIPopupList mList;

    private void Awake()
    {
        this.mList = base.GetComponent<UIPopupList>();
        this.mCheck = base.GetComponent<UICheckbox>();
        if (this.mList != null)
        {
            this.mList.onSelectionChange = (UIPopupList.OnSelectionChange) Delegate.Combine(this.mList.onSelectionChange, new UIPopupList.OnSelectionChange(this.SaveSelection));
        }
        if (this.mCheck != null)
        {
            this.mCheck.onStateChange = (UICheckbox.OnStateChange) Delegate.Combine(this.mCheck.onStateChange, new UICheckbox.OnStateChange(this.SaveState));
        }
    }

    private void OnDestroy()
    {
        if (this.mCheck != null)
        {
            this.mCheck.onStateChange = (UICheckbox.OnStateChange) Delegate.Remove(this.mCheck.onStateChange, new UICheckbox.OnStateChange(this.SaveState));
        }
        if (this.mList != null)
        {
            this.mList.onSelectionChange = (UIPopupList.OnSelectionChange) Delegate.Remove(this.mList.onSelectionChange, new UIPopupList.OnSelectionChange(this.SaveSelection));
        }
    }

    private void OnDisable()
    {
        if ((this.mCheck == null) && (this.mList == null))
        {
            UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                UICheckbox checkbox = componentsInChildren[index];
                if (checkbox.isChecked)
                {
                    this.SaveSelection(checkbox.name);
                    break;
                }
                index++;
            }
        }
    }

    private void OnEnable()
    {
        if (this.mList != null)
        {
            string str = PlayerPrefs.GetString(this.key);
            if (!string.IsNullOrEmpty(str))
            {
                this.mList.selection = str;
            }
        }
        else if (this.mCheck != null)
        {
            this.mCheck.isChecked = PlayerPrefs.GetInt(this.key, 1) != 0;
        }
        else
        {
            string str2 = PlayerPrefs.GetString(this.key);
            UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                UICheckbox checkbox = componentsInChildren[index];
                checkbox.isChecked = checkbox.name == str2;
                index++;
            }
        }
    }

    private void SaveSelection(string selection)
    {
        PlayerPrefs.SetString(this.key, selection);
    }

    private void SaveState(bool state)
    {
        PlayerPrefs.SetInt(this.key, !state ? 0 : 1);
    }

    private string key
    {
        get
        {
            return (!string.IsNullOrEmpty(this.keyName) ? this.keyName : ("NGUI State: " + base.name));
        }
    }
}

