using System;
using UnityEngine;

public class BTN_TO_REGISTER : MonoBehaviour
{
    public GameObject registerPanel;

    private void OnClick()
    {
        NGUITools.SetActive(base.transform.parent.gameObject, false);
        NGUITools.SetActive(this.registerPanel, true);
    }
}

