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
        public float distanceScale;


        public float timeToDie;
        public float defaultTimeToFade = 1f;
        float timeToFade = 1f;
        public float fadingAlpha;

        SfxVisualizer sfxVisualizer;

        private GameObject markerGO;


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
            position = new Vector2(transform.position.x, transform.position.z);
        }

        void Update()
        {
            if(timeToDie > 0)
            {
                timeToDie -= Time.deltaTime;
            } else if(timeToDie <= 0)
            {
                sfxVisualizer.DeleteSfxMarker(this);
            }

            
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
            markerGO.GetComponent<Image>().color = new Color(1,1,1,fadingAlpha * 0.8f);
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
