using System;
using UnityEngine;

public class BTN_SSR_up : MonoBehaviour
{
    public GameObject panel;

    private void OnClick()
    {
        this.panel.GetComponent<SnapShotReview>().ShowPrevIMG();
    }
}

