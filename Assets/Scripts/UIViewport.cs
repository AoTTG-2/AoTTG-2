using System;
using UnityEngine;

[RequireComponent(typeof(Camera)), ExecuteInEditMode, AddComponentMenu("NGUI/UI/Viewport Camera")]
public class UIViewport : MonoBehaviour
{
    public Transform bottomRight;
    public float fullSize = 1f;
    private Camera mCam;
    public Camera sourceCamera;
    public Transform topLeft;

    private void LateUpdate()
    {
        if ((this.topLeft != null) && (this.bottomRight != null))
        {
            Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
            Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
            Rect rect = new Rect(vector.x / ((float) Screen.width), vector2.y / ((float) Screen.height), (vector2.x - vector.x) / ((float) Screen.width), (vector.y - vector2.y) / ((float) Screen.height));
            float num = this.fullSize * rect.height;
            if (rect != this.mCam.rect)
            {
                this.mCam.rect = rect;
            }
            if (this.mCam.orthographicSize != num)
            {
                this.mCam.orthographicSize = num;
            }
        }
    }

    private void Start()
    {
        this.mCam = base.GetComponent<Camera>();
        if (this.sourceCamera == null)
        {
            this.sourceCamera = Camera.main;
        }
    }
}

