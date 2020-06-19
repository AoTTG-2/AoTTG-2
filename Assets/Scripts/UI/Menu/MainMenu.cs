namespace Assets.Scripts.UI.Menu
{
    public class MainMenu : UiNavigationElement
    {
        public void Singleplayer()
        {
            Navigate(typeof(Singleplayer));
        }

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
