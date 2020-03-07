using System;
using UnityEngine;

public class CameraForLeftEye : MonoBehaviour
{
    private new Camera camera;
    private Camera cameraRightEye;
    public GameObject rightEye;

    private void LateUpdate()
    {
        this.camera.aspect = this.cameraRightEye.aspect;
        this.camera.fieldOfView = this.cameraRightEye.fieldOfView;
    }

    private void Start()
    {
        this.camera = base.GetComponent<Camera>();
        this.cameraRightEye = this.rightEye.GetComponent<Camera>();
    }
}

