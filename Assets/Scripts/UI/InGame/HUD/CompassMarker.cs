using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class CompassMarker : MonoBehaviour
    {
        public UnityEngine.Sprite icon;
        public Image image;

        public Vector2 position;

        void Start()
        {
            GameObject.Find("Compass").GetComponent<CompassController>().AddCompassMarker(this);
            position = new Vector2(transform.position.x, transform.position.z);
        }


        void OnDestroy()
        {
            GameObject.Find("Compass").GetComponent<CompassController>().DeleteCompassMarker(this);
        }

    }
}
