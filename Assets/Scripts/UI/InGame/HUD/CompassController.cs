using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class CompassController : UiElement
    {
        public bool compassMode;
        public RawImage compassImage;
        public Transform cam;
        public float screenPositionRatio = 0.95f;


        void OnEnable()
        {
            Debug.Log("Has custom hud?: "+PlayerPrefs.GetInt("hasCustomHUD", 0)); // TODO: Remove this when finished. 
            if(PlayerPrefs.GetInt("hasCustomHUD", 0) != 1)
            {
                GameObject.Find("CompassContainer").transform.position = new Vector3(Screen.width * 0.5f, Screen.height * screenPositionRatio, 0f);
            }
        }

        void Update()
        {
            if(compassMode)
            {
                compassImage.uvRect = new Rect (cam.localEulerAngles.y / 360f, 0f, 1f, 1f);
            }
        }
    }
}
