using Assets.Scripts.Room.Chat;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.InGame.Controls;
using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : UiContainer
    {
        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenuV2 SpawnMenu;
        public GraphicSettingMenu GraphicSettingMenu;
        public ControlsMenu ControlsMenu;
        public PauseIndicator PauseIndicator;
        private static IPauseService PauseService => Service.Pause;

        public void TogglePauseMenu()
        {
            if (Menu.IsVisible() && GetNumVisibleChildMenus() > 0 )
            {
                Menu.Hide();
                if (PhotonNetwork.offlineMode)
                {
                    PauseService.Pause(false, true);
                }
            }
            else if (!Menu.IsVisible() && GetNumVisibleChildMenus() == 0)
            {
                Menu.Show();
                if (PhotonNetwork.offlineMode)
                {
                    PauseService.Pause(true, true);
                }
            }
        }

        private void Awake()
        {
            AddChild(HUD);
            AddChild(Menu);
            AddChild(SpawnMenu);
            AddChild(GraphicSettingMenu);
            AddChild(ControlsMenu);
            AddChild(PauseIndicator);
        }

        private int GetNumVisibleChildMenus()
        {
            return GetChildren().FindAll(e => e.IsVisible() && e is UiMenu).Count;
        }

        private void OnEnable()
        {
            HUD.Show();
            SpawnMenu.Show();
            GraphicSettingMenu.Hide();
            Menu.Hide();
            PauseIndicator.Hide();
            ControlsMenu.Hide();

            PauseService.OnPaused += PauseService_OnPaused;
            PauseService.OnUnPaused += PauseService_OnUnPaused;
        }

        private void PauseService_OnPaused(object sender, System.EventArgs e) => PauseIndicator.Pause();
        private void PauseService_OnUnPaused(object sender, System.EventArgs e) => PauseIndicator.UnPause();

        private void Update()
        {
            // So for the first issue, the Pause stuff was never meant to interface with the pause command...

            // The Escape key unlocks the cursor in the editor,
            // which is why exiting the menu messes with TPS.
            // So when you press p whilst you are typing it jusat doesn't wor for some reason...
            if (UnityEngine.Input.GetKeyDown(InputManager.Menu) && !MenuManager.IsMenuOpen(typeof(InRoomChat)))
            {
                TogglePauseMenu();
            }
        }

        private void OnDestroy()
        {
            PauseService.OnPaused -= PauseService_OnPaused;
            PauseService.OnUnPaused -= PauseService_OnUnPaused;
        }
    }
}