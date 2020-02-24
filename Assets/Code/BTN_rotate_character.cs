using System;
using UnityEngine;

public class BTN_rotate_character : MonoBehaviour
{
    public new GameObject camera;
    private float distance = 3f;
    public GameObject hero;
    private bool isRotate;

    private void OnPress(bool press)
    {
        if (press)
        {
            this.isRotate = true;
        }
        else
        {
            this.isRotate = false;
        }
    }

    private void Update()
    {
        this.distance -= Input.GetAxis("Mouse ScrollWheel") * 0.05f;
        this.distance = Mathf.Clamp(this.distance, 0.8f, 3.5f);
        this.camera.transform.position = this.hero.transform.position;
        Transform transform = this.camera.transform;
        transform.position += (Vector3) (Vector3.up * 1.1f);
        if (this.isRotate)
        {
            float angle = Input.GetAxis("Mouse X") * 2.5f;
            float num2 = -Input.GetAxis("Mouse Y") * 2.5f;
            this.camera.transform.RotateAround(this.camera.transform.position, Vector3.up, angle);
            this.camera.transform.RotateAround(this.camera.transform.position, this.camera.transform.right, num2);
        }
        Transform transform2 = this.camera.transform;
        transform2.position -= (Vector3) (this.camera.transform.forward * this.distance);
    }
}

