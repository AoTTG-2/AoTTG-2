using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class SfxVisualizer : UiElement
    {
        public GameObject sfxMarkerPrefab;
        public List<SfxMarker> sfxMarkers = new List<SfxMarker>();
        public float ringUnit;

        public Transform cam;
        public Transform player;
        public float maxDistance;

        public bool visualize;
        
        void Start()
        {

        }

        void Update()
        {
            if(visualize)
            {
            // ringElement.transform.rotation = Quaternion.Euler(0,0,ringUnit);
                foreach(SfxMarker marker in sfxMarkers)
                {
                    marker.image.transform.rotation = Quaternion.Euler(0,0,GetUnit(marker));
                    Debug.Log(GetUnit(marker));

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

        public void AddSfxMarker (SfxMarker marker)
        {
            GameObject newMarker = Instantiate(sfxMarkerPrefab, transform);
            newMarker.name = marker.markerID;
            marker.image = newMarker.GetComponent<Image>();
            marker.image.sprite = marker.icon;

            sfxMarkers.Add(marker);
        }

        public void DeleteCompassMarker(SfxMarker marker)
        {
            foreach(Transform child in transform)
            {
                if(child.name == marker.markerID)
                {
                    Destroy(child.gameObject);
                }
            }
            
            sfxMarkers.Remove(marker);
        }

        float GetUnit (SfxMarker marker)
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 playerFwd = new Vector2(cam.transform.forward.x, cam.transform.forward.z);

            float angle = Vector2.SignedAngle (marker.position - playerPos, playerFwd);
        
            return angle*-1;
        }
    }
}
