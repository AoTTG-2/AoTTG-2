using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Lag Position")]
public class LagPosition : MonoBehaviour
{
    public bool ignoreTimeScale;
    private Vector3 mAbsolute;
    private Vector3 mRelative;
    private Transform mTrans;
    public Vector3 speed = new Vector3(10f, 10f, 10f);
    public int updateOrder;

    private void CoroutineUpdate(float delta)
    {
        Transform parent = this.mTrans.parent;
        if (parent != null)
        {
            Vector3 vector = parent.position + (parent.rotation * this.mRelative);
            this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector.x, Mathf.Clamp01(delta * this.speed.x));
            this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector.y, Mathf.Clamp01(delta * this.speed.y));
            this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector.z, Mathf.Clamp01(delta * this.speed.z));
            this.mTrans.position = this.mAbsolute;
        }
    }

    private void OnEnable()
    {
        this.mTrans = base.transform;
        this.mAbsolute = this.mTrans.position;
    }

    private void Start()
    {
        this.mTrans = base.transform;
        this.mRelative = this.mTrans.localPosition;
        if (this.ignoreTimeScale)
        {
            UpdateManager.AddCoroutine(this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
        }
        else
        {
            UpdateManager.AddLateUpdate(this, this.updateOrder, new UpdateManager.OnUpdate(this.CoroutineUpdate));
        }
    }
}

