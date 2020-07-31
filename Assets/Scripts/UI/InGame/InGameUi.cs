using Assets.Scripts.UI.InGame.Controls;
using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : MonoBehaviour
    {
        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenu SpawnMenu;
        public GraphicSettingMenu GraphicSettingMenu;
        public ControlsMenu ControlsMenu;
        public PauseIndicator PauseIndicator;

        private static int _activeMenus;

        /// <summary>
        /// Toggles the Pause Indicator
        /// </summary>
        /// <param name="state">true to toggle it active</param>
        public void ToggleIndicator(bool state)
        {
            if (state)
            {
                PauseIndicator.Pause();
            }
            else {
                PauseIndicator.UnPause();
            }
        }

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
        }

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
                        FengGameManagerMKII.instance.pauseWaitTime = 0.0f;
                    }
                } else if (!Menu.gameObject.activeSelf && !MenuManager.IsMenuOpen)
                {
                    Menu.gameObject.SetActive(true);
                    if (PhotonNetwork.offlineMode)
                    {
                        FengGameManagerMKII.instance.pauseWaitTime = 100000f;
                        Time.timeScale = 1E-06f;
                    }
                }
            }
        }
    }
}