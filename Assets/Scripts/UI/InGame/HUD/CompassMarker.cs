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


        void Awake()
        {
            //Generate a unique ID as a name for the compass controller to remove this exact marker and not another marker with the same name.
            System.Guid myGUID = System.Guid.NewGuid();
            markerID = myGUID.ToString();
        }

        void Start()
        {
            if(isFlare)
            {
                ParticleSystem particle = GetComponent<ParticleSystem>();
                markerColor = particle.main.startColor.color;
            }
            CompassController compass = GameObject.Find("Compass").GetComponent<CompassController>();
            compass.AddCompassMarker(this);
            position = new Vector2(transform.position.x, transform.position.z);
        }

        void Update()
        {
            if(followAlways)
            {
                position = new Vector2(transform.position.x, transform.position.z);
            }
        }


        void OnDestroy()
        {
            GameObject.Find("Compass").GetComponent<CompassController>().DeleteCompassMarker(this);
        }

    }
}
