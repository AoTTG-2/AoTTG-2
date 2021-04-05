using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class Labels : UiElement
    {
        public Text Center;
        public Text Top;
        public Text TopRight;
        public Text TopLeft;

        public void Awake()
        {
            Center.text
                = Top.text
                = TopRight.text
                = TopLeft.text
                = "";
        }
    }
}
