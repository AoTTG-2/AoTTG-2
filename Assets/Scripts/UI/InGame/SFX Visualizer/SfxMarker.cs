using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.UI.InGame
{
    public class SfxMarker : UiElement
    {
        [Header("Customize")]
        public UnityEngine.Sprite icon;
        public Color markerColor;
        [Header("Settings")]
        public bool triggerOnStart;
        public bool isGlobal;
        public bool followAlways;
        public float defaultTimeToFade = 1f;

        [HideInInspector]public Image image;
        [HideInInspector]public Vector2 position;
        [HideInInspector]public string markerID;
        [HideInInspector]public float distanceScale;
        float timeToFade = 1f;
        [HideInInspector]public float fadingAlpha;
        SfxVisualizer sfxVisualizer;
        private GameObject markerGO;
        [HideInInspector]public GameObject markerGOicon;


        void Awake()
        {
            System.Guid myGUID = System.Guid.NewGuid();
            markerID = myGUID.ToString();
        }

        void Start()
        {
            sfxVisualizer = GameObject.Find("SFX Visualizer").GetComponent<SfxVisualizer>();
            sfxVisualizer.AddSfxMarker(this);
            markerGO = GameObject.Find(markerID);
            markerGOicon = markerGO.transform.GetChild(0).gameObject;
            position = new Vector2(transform.position.x, transform.position.z);
            if(triggerOnStart) TriggerSound();
        }

        void Update()
        {
            if(timeToFade > 0)
            {
                timeToFade -= Time.deltaTime;
            } else if(timeToFade <= 0)
            {
                return;
            }
                
            if(followAlways)
            {
                position = new Vector2(transform.position.x, transform.position.z);
            }

            fadingAlpha = distanceScale * timeToFade/defaultTimeToFade;
            markerColor.a = fadingAlpha * 0.8f;
            markerGO.GetComponent<Image>().color = markerColor;
            markerGOicon.GetComponent<Image>().color = new Color(1,1,1,fadingAlpha);
        }

        public void TriggerSound()
        {
            timeToFade = defaultTimeToFade;
            markerGO.GetComponent<Animator>().SetTrigger("Shake");
        }


        void OnDestroy()
        {
            GameObject.Find("SFX Visualizer").GetComponent<SfxVisualizer>().DeleteSfxMarker(this);
        }
    }
}
