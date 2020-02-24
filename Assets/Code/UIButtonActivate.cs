using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
    public bool state = true;
    public GameObject target;

    private void OnClick()
    {
        if (this.target != null)
        {
            NGUITools.SetActive(this.target, this.state);
        }
    }
}

