using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : MonoBehaviour
    {
        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenu SpawnMenu;

        private static int _activeMenus;

        public static void OnMenuOpened()
        {
            Debug.Log("Menu opened");
            _activeMenus++;
        }

        public static void OnMenuClosed()
        {
            Debug.Log("Menu closed");
            if (_activeMenus == 0)
            {
                Debug.LogError("Tried to close a menu while there was none");
            }
            else
            {
                _activeMenus--;
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
            Menu.gameObject.SetActive(false);
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                Menu.gameObject.SetActive(!Menu.isActiveAndEnabled);
            }
        }
    }
}