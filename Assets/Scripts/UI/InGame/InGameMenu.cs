using Assets.Scripts.UI.InGame.Controls;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : UiMenu
    {
        public GameSettingMenu GameSettingsMenu;
        public ControlsMenu ControlsMenu;
        public GraphicSettingMenu GraphicsSettingsMenu;

        // Used by Button.
        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }

        // Used by Button.
        public void ShowGameSettingsMenu()
        {
            GameSettingsMenu.Show();
        }

        // Used by Button.
        public void ShowGraphicSettingsMenu()
        {
            GraphicsSettingsMenu.Show();
        }

        // Used by Button.
        public void ShowRebindsMenu()
        {
            ControlsMenu.Show();
        }

        private void Awake()
        {
            AddChild(GameSettingsMenu);
            AddChild(ControlsMenu);
            AddChild(GraphicsSettingsMenu);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameSettingsMenu.Hide();
            GraphicsSettingsMenu.Hide();
            ControlsMenu.Hide();
        }
    }
}
