using System;
using UnityEngine;

public class BTN_SSR_Down : MonoBehaviour
{
    public GameObject panel;

    private void OnClick()
    {
        this.panel.GetComponent<SnapShotReview>().ShowNextIMG();
    }
}

