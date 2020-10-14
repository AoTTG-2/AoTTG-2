using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public Axis axis;
    public bool reverseFace;
    
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
        if (Camera.main == null) return;
        var worldPosition = base.transform.position + (Camera.main.transform.rotation * (!this.reverseFace ? Vector3.back : Vector3.forward));
        var worldUp = Camera.main.transform.rotation * this.GetAxis(this.axis);
        transform.LookAt(worldPosition, worldUp);
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

