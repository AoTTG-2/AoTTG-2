using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class CompassMarker : MonoBehaviour
    {
        public UnityEngine.Sprite icon;
        public Image image;

        public Vector2 position;

        public string markerID;

        public bool isGlobal;
        public bool followAlways;

        void Awake()
        {
            //Generate a unique ID as a name for the compass controller to remove this exact marker and not another marker with the same name.
            System.Guid myGUID = System.Guid.NewGuid();
            markerID = myGUID.ToString();
        }

        void Start()
        {
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
