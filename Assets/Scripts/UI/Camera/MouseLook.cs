using UnityEngine;

namespace Assets.Scripts.UI.Camera
{
    [AddComponentMenu("Camera-Control/Mouse Look")]
    public class MouseLook : MonoBehaviour
    {
        public bool disable;
        Vector2 rotation = new Vector2(0, 0);
        public float speed = 3;

        void Update()
        {
            if (!this.disable)
            {
                rotation.y += UnityEngine.Input.GetAxis("Mouse X");
                rotation.x += -UnityEngine.Input.GetAxis("Mouse Y");
                transform.eulerAngles = rotation * speed;
            }
        }
    }
}