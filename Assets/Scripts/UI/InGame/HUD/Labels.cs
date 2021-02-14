using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class Labels : UiElement
    {
        private float screenWidth;

        public TMP_Text Center;
        public TMP_Text Top;
        public TMP_Text TopRight;
        public TMP_Text TopLeft;

        public void Awake()
        {
            Center.text
                = Top.text
                = TopRight.text
                = TopLeft.text
                = "";
        }

        void Start()
        {
            SetLocations();
            screenWidth = Screen.width;
        }

        void Update()
        {   
            // Check if the resolution changed. 
            if(screenWidth != Screen.width){
                SetLocations();
                screenWidth = Screen.width;
            }
        }

        private void SetLocations()
        {
            Debug.Log("SetLocations() called");
            TopLeft.transform.position = new Vector3(Screen.width * 0.1f, Screen.height * 0.87f, 0f);
            TopLeft.gameObject.SetActive(false);
            TopLeft.gameObject.SetActive(true);
            TopRight.transform.position = new Vector3(Screen.width * 0.9f, Screen.height * 0.87f, 0f);
            TopRight.gameObject.SetActive(false);
            TopRight.gameObject.SetActive(true);
            Top.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.93f, 0f);
            Top.gameObject.SetActive(false);
            Top.gameObject.SetActive(true);
        }
    }
}
