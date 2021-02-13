using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SortLabelIndicator : MonoBehaviour
{
    public string defaultText;
    public TMP_Text myText;

    public void SetIndicator()
    {
        if(!myText.text.Contains("↓")) myText.text += " ↓";
    }

    public void SetDefault()
    {
        myText.text = defaultText;
    }
}
