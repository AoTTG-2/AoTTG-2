using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class CompassMarker : MonoBehaviour
    {
        [Header("Customize")]
        public UnityEngine.Sprite icon;
        public Color markerColor;
        [Header("Settings")]
        public bool isFlare; //Get Particle System Color
        public bool isGlobal;
        public bool followAlways;


        [HideInInspector]public Image image;
        [HideInInspector]public Vector2 position;
        [HideInInspector]public string markerID;


        private void Awake()
        {
            //Generate a unique ID as a name for the compass controller to remove this exact marker and not another marker with the same name.
            System.Guid myGUID = System.Guid.NewGuid();
            markerID = myGUID.ToString();
            Service.Level.OnLevelLoaded += Level_OnLevelLoaded;
        }

        private void Level_OnLevelLoaded(int scene, Room.Level level)
        {
            if (isFlare)
            {
                ParticleSystem particle = GetComponent<ParticleSystem>();
                markerColor = particle.main.startColor.color;
            }
            CompassController compass = GameObject.Find("Compass").GetComponent<CompassController>();
            compass.AddCompassMarker(this);
            position = new Vector2(transform.position.x, transform.position.z);
        }


        private void Update()
        {
            if(followAlways)
            {
                position = new Vector2(transform.position.x, transform.position.z);
            }
        }


        private void OnDestroy()
        {
            Service.Level.OnLevelLoaded -= Level_OnLevelLoaded;
            var compass = GameObject.Find("Compass");
            if (compass == null) return;
            compass.GetComponent<CompassController>()?.DeleteCompassMarker(this);
        }

    }
}
