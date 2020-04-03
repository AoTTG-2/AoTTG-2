namespace Assets.Scripts.UI.Menu
{
    public class MainMenu : UiElement
    {
        public void Multiplayer()
        {
            Navigate(typeof(Lobby));
        }

        public void Credits()
        {
            Navigate(typeof(Credits));
        }

        public void MapEditor()
        {
            //TODO: Switch to MapEditor scene
        }
    }
}
