using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class MouseLook : MonoBehaviour
{
    public bool disable;
    Vector2 rotation = new Vector2(0, 0);
    public float speed = 3;

    void Update()
    {
        if (!this.disable)
        {
            rotation.y += Input.GetAxis("Mouse X");
            rotation.x += -Input.GetAxis("Mouse Y");
            transform.eulerAngles = (Vector2) rotation * speed;
        }
    }
}