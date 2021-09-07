using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class MinimapCamera : MonoBehaviour
    {
        public int Height = 500;
        public bool Rotate = true;
        public IN_GAME_MAIN_CAMERA mainCamera;
        public GameObject playerTransform;
        public UnityCamera minimapCamera;

        private float shadowDistance;
        private void OnEnable()
        {
            mainCamera = FindObjectOfType<IN_GAME_MAIN_CAMERA>();
            if (mainCamera != null)
                playerTransform = mainCamera.main_object;
            minimapCamera = GetComponent<UnityCamera>();
        }

        private void LateUpdate()
        {
            if (mainCamera == null)
            {
                mainCamera = FindObjectOfType<IN_GAME_MAIN_CAMERA>();
                return;
            }
            else if (playerTransform == null)
            {
                playerTransform = mainCamera.main_object;
                return;
            }
            else if (minimapCamera == null)
            {
                minimapCamera = GetComponent<UnityCamera>();
                return;
            }

            var pos = playerTransform.transform.position;
            var rot = mainCamera.transform.rotation;
            minimapCamera.orthographicSize = Height;
            transform.position = new Vector3(pos.x, transform.position.y, pos.z);
            transform.eulerAngles = Rotate
                ? new Vector3(90, rot.eulerAngles.y)
                : new Vector3(90, 0);
        }

        private void OnPreRender()
        {
            shadowDistance = QualitySettings.shadowDistance;
            QualitySettings.shadowDistance = 0;
        }

        private void OnPostRender()
        {
            QualitySettings.shadowDistance = shadowDistance;
        }
    }
}
