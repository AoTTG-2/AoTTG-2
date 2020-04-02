using UnityEngine;

namespace Assets.Scripts.Room.Minimap
{
    public class MinimapCamera : MonoBehaviour
    {
        public int Height = 500;
        public bool Rotate = true;
        private GameObject mainCamera;
        private Camera minimapCamera;

        void OnEnable()
        {
            mainCamera = GameObject.Find("MainCamera");
            minimapCamera = gameObject.GetComponent<Camera>();
        }
        
        public void FixedUpdate()
        {
            if (mainCamera == null)
            {
                OnEnable();
            }
            var mainCameraVector = mainCamera.transform.position;
            var mainCameraRotation = mainCamera.transform.rotation;
            minimapCamera.orthographicSize = Height;
            transform.position = new Vector3(mainCameraVector.x, mainCameraVector.y + Height, mainCameraVector.z);
            transform.eulerAngles = Rotate
                ? new Vector3(90, mainCameraRotation.eulerAngles.y)
                : new Vector3(90, 0); 
            //transform.rotation = Rotate
            //    ? new Quaternion(90f, mainCameraRotation.y, transform.rotation.z, transform.rotation.w)
            //    : new Quaternion(90f, 0f, transform.rotation.z, transform.rotation.w);
        }
    }
}
