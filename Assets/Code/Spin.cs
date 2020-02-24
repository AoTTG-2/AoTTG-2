using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
    private Rigidbody mRb;
    private Transform mTrans;
    public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

    public void ApplyDelta(float delta)
    {
        delta *= 360f;
        Quaternion quaternion = Quaternion.Euler((Vector3) (this.rotationsPerSecond * delta));
        if (this.mRb == null)
        {
            this.mTrans.rotation *= quaternion;
        }
        else
        {
            this.mRb.MoveRotation(this.mRb.rotation * quaternion);
        }
    }

    private void FixedUpdate()
    {
        if (this.mRb != null)
        {
            this.ApplyDelta(Time.deltaTime);
        }
    }

    private void Start()
    {
        this.mTrans = base.transform;
        this.mRb = base.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (this.mRb == null)
        {
            this.ApplyDelta(Time.deltaTime);
        }
    }
}

