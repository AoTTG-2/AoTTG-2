using System;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public Axis axis;
    private Camera referenceCamera;
    public bool reverseFace;

    private void Awake()
    {
        if (this.referenceCamera == null)
        {
            this.referenceCamera = Camera.main;
        }
    }

    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.down:
                return Vector3.down;

            case Axis.left:
                return Vector3.left;

            case Axis.right:
                return Vector3.right;

            case Axis.forward:
                return Vector3.forward;

            case Axis.back:
                return Vector3.back;
        }
        return Vector3.up;
    }

    private void Update()
    {
        Vector3 worldPosition = base.transform.position + (this.referenceCamera.transform.rotation * (!this.reverseFace ? Vector3.back : Vector3.forward));
        Vector3 worldUp = (Vector3) (this.referenceCamera.transform.rotation * this.GetAxis(this.axis));
        base.transform.LookAt(worldPosition, worldUp);
    }

    public enum Axis
    {
        up,
        down,
        left,
        right,
        forward,
        back
    }
}

