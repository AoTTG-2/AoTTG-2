using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class Labels : MonoBehaviour
    {
        public Text Center;
        public Text Top;
        public Text TopRight;
        public Text TopLeft;

        public void OnEnable()
        {
            Center.text
                = Top.text
                = TopRight.text
                = TopLeft.text
                = "";
        }
    }
}
