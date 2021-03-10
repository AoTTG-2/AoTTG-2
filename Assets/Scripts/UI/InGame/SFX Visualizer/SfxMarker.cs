using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.UI.InGame
{
    public class SfxMarker : UiElement
    {
        public UnityEngine.Sprite icon;
        public Image image;

        public Vector2 position;

        public string markerID;

        public bool isGlobal;
        public bool followAlways;

        void Awake()
        {
            System.Guid myGUID = System.Guid.NewGuid();
            markerID = myGUID.ToString();
        }

        void Start()
        {
            SfxVisualizer sfxVisualizer = GameObject.Find("SFX Visualizer").GetComponent<SfxVisualizer>();
            sfxVisualizer.AddSfxMarker(this);
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
            GameObject.Find("SFX Visualizer").GetComponent<SfxVisualizer>().DeleteCompassMarker(this);
        }
    }
}
