using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class CompassController : UiElement
    {
        [Header("Customization")]
        public RawImage compassImage;
        [Header("Settings")]
        public float maxDistance; 
        public GameObject iconPrefab;
        public GameObject markersParent;
        [Header("Markers")]
        public List<CompassMarker> compassMarkers = new List<CompassMarker>();
        [HideInInspector]public bool compassMode;
        [HideInInspector]public Transform cam; //Is declared by the IN_GAME_MAIN_CAMERA script



        float compassUnit;

        void Start()
        {
            //1 compass unit equals 1 degree angle
            compassUnit = compassImage.rectTransform.rect.width / 360f;
            maxDistance = 150f; //minimap max distance
        }

        void Update()
        {
            if(compassMode)
            {
                compassImage.uvRect = new Rect (cam.localEulerAngles.y / 360f, 0f, 1f, 1f);

                foreach(CompassMarker marker in compassMarkers)
                {
                    marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                    if(!marker.isGlobal)
                    {
                        float dst = Vector2.Distance (new Vector2(cam.transform.position.x, cam.transform.position.z), marker.position);
                        float scale = 0f;

                        if (dst < maxDistance)
                            scale = 1f - (dst / maxDistance);

                        //Fades whenever the player is far away
                        var tempColor = marker.image.color;
                        tempColor.a = scale;
                        marker.image.color = tempColor;
                    }
                } 

            }

        }

        public void AddCompassMarker (CompassMarker marker)
        {
            GameObject newMarker = Instantiate(iconPrefab, markersParent.transform);
            newMarker.name = marker.markerID;
            marker.image = newMarker.GetComponent<Image>();
            marker.image.sprite = marker.icon;
            marker.image.color = marker.markerColor;

            compassMarkers.Add(marker);
        }

        public void DeleteCompassMarker(CompassMarker marker)
        {
            foreach(Transform child in markersParent.transform)
            {
                if(child.name == marker.markerID)
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
