using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.InGame.Controls;
using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : MonoBehaviour
    {
        private static IPauseService PauseService => Service.Pause;

        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenu SpawnMenu;
        public GraphicSettingMenu GraphicSettingMenu;
        public ControlsMenu ControlsMenu;
        public PauseIndicator PauseIndicator;

        private static int _activeMenus;

        public static bool IsMenuOpen()
        {
            return _activeMenus > 0;
        }

        void OnEnable()
        {
            HUD.gameObject.SetActive(true);
            SpawnMenu.gameObject.SetActive(true);
            GraphicSettingMenu.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
            ControlsMenu.gameObject.SetActive(false);

            PauseService.OnPaused += PauseService_OnPaused;
            PauseService.OnUnPaused += PauseService_OnUnPaused;
        }
        private void PauseService_OnPaused(object sender, System.EventArgs e) => PauseIndicator.Pause();
        private void PauseService_OnUnPaused(object sender, System.EventArgs e) => PauseIndicator.UnPause();

        private void Update()
        {
            // The Escape key unlocks the cursor in the editor,
            // which is why exiting the menu messes with TPS.
            if (UnityEngine.Input.GetKeyDown(InputManager.Menu))
            {
                if (Menu.gameObject.activeSelf && MenuManager.IsMenuOpen)
                {
                    Menu.gameObject.SetActive(false);
                    if (PhotonNetwork.offlineMode)
                    {
                        PauseIndicator.ShowUi = true;
                        PauseService.Pause(false, true);
                    }
                }
                else if (!Menu.gameObject.activeSelf && !MenuManager.IsMenuOpen)
                {
                    Menu.gameObject.SetActive(true);
                    if (PhotonNetwork.offlineMode)
                    {
                        PauseIndicator.ShowUi = false;
                        PauseService.Pause(true);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            PauseService.OnPaused -= PauseService_OnPaused;
            PauseService.OnUnPaused -= PauseService_OnUnPaused;
        }
    }
}