using System;
using UnityEngine;

/// <summary>
/// The script for the rock that is thrown after the Trost gate is destroyed on the Colossal Titan map
/// </summary>
public class RockScript : MonoBehaviour
{
    private Vector3 desPt = new Vector3(-200f, 0f, -280f);
    private bool disable;
    private float g = 500f;
    private float speed = 800f;
    private Vector3 vh;
    private Vector3 vv;

    private void Start()
    {
        base.transform.position = new Vector3(0f, 0f, 676f);
        this.vh = this.desPt - base.transform.position;
        this.vv = new Vector3(0f, (this.g * this.vh.magnitude) / (2f * this.speed), 0f);
        this.vh.Normalize();
        this.vh = (Vector3) (this.vh * this.speed);
    }

    private void Update()
    {
        if (!this.disable)
        {
            this.vv += (Vector3) ((-Vector3.up * this.g) * Time.deltaTime);
            Transform transform = base.transform;
            transform.position += (Vector3) (this.vv * Time.deltaTime);
            Transform transform2 = base.transform;
            transform2.position += (Vector3) (this.vh * Time.deltaTime);
            if ((Vector3.Distance(this.desPt, base.transform.position) < 20f) || (base.transform.position.y < 0f))
            {
                base.transform.position = this.desPt;
                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.Instantiate("FX/boom1_CT_KICK", base.transform.position + ((Vector3) (Vector3.up * 30f)), Quaternion.Euler(270f, 0f, 0f), 0);
                }
                else
                {
                    UnityEngine.Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), base.transform.position + ((Vector3) (Vector3.up * 30f)), Quaternion.Euler(270f, 0f, 0f));
                }
                this.disable = true;
            }
        }
    }
}

