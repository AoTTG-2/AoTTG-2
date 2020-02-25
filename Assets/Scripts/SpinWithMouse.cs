using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
    private Transform mTrans;
    public float speed = 1f;
    public Transform target;

    private void OnDrag(Vector2 delta)
    {
        UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
        if (this.target != null)
        {
            this.target.localRotation = Quaternion.Euler(0f, (-0.5f * delta.x) * this.speed, 0f) * this.target.localRotation;
        }
        else
        {
            this.mTrans.localRotation = Quaternion.Euler(0f, (-0.5f * delta.x) * this.speed, 0f) * this.mTrans.localRotation;
        }
    }

    private void Start()
    {
        this.mTrans = base.transform;
    }
}

