using TMPro;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class Labels : UiElement
    {
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
    }
}
