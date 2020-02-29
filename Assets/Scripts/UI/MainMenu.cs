using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenu : UiElement
    {
        public void Multiplayer()
        {
            Navigate(typeof(ServerRegion));
        }

        public void Credits()
        {
            Navigate(typeof(Credits));
        }
    }
}
