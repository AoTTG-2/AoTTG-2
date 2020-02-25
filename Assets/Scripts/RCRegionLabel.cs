using System;
using UnityEngine;

public class RCRegionLabel : MonoBehaviour
{
    public GameObject myLabel;

    private void Update()
    {
        if ((this.myLabel != null) && this.myLabel.GetComponent<UILabel>().isVisible)
        {
            this.myLabel.transform.LookAt(((Vector3) (2f * this.myLabel.transform.position)) - Camera.main.transform.position);
        }
    }
}

