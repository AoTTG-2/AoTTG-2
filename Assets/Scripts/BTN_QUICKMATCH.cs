using System;
using UnityEngine;

public class BTN_QUICKMATCH : MonoBehaviour
{
    private void OnClick()
    {
    }

    private void Start()
    {
        base.gameObject.GetComponent<UIButton>().isEnabled = false;
    }
}

