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
        }

        private void Update()
        {
            // The Escape key unlocks the cursor in the editor,
            // which is why exiting the menu messes with TPS.
            if (UnityEngine.Input.GetKeyDown(InputManager.Menu))
                Menu.gameObject.SetActive(!Menu.isActiveAndEnabled);
        }
    }
}