using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class CompassController : UiElement
    {
        public GameObject iconPrefab;
        public List<CompassMarker> compassMarkers = new List<CompassMarker>();

        public bool compassMode;

        public RawImage compassImage;
        public Transform cam;

        float compassUnit;

        void Start()
        {
            //1 compass unit equals 1 degree angle
            compassUnit = compassImage.rectTransform.rect.width / 360f;
        }

        void Update()
        {
            if(compassMode)
            {
                compassImage.uvRect = new Rect (cam.localEulerAngles.y / 360f, 0f, 1f, 1f);

                foreach(CompassMarker marker in compassMarkers)
                {
                    marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);
                } 

            }

        }

        public void AddCompassMarker (CompassMarker marker)
        {
            GameObject newMarker = Instantiate(iconPrefab, compassImage.transform);
            newMarker.name = marker.name;
            marker.image = newMarker.GetComponent<Image>();
            marker.image.sprite = marker.icon;

            compassMarkers.Add(marker);
        }

        public void DeleteCompassMarker(CompassMarker marker)
        {
            foreach(Transform child in compassImage.transform)
            {
                if(child.name == marker.name)
                {
                    Destroy(child.gameObject);
                }
            }
            
            compassMarkers.Remove(marker);
        }

        Vector2 GetPosOnCompass (CompassMarker marker)
        {
            Vector2 playerPos = new Vector2(cam.transform.position.x, cam.transform.position.z);
            Vector2 playerFwd = new Vector2(cam.transform.forward.x, cam.transform.forward.z);

            float angle = Vector2.SignedAngle (marker.position - playerPos, playerFwd);

            return new Vector2(compassUnit * angle, 0f);
        }
    }
}
