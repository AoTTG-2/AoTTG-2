using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : MonoBehaviour
    {
        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenu SpawnMenu;
        public GraphicSettingMenu GraphicSettingMenu;

        void OnEnable()
        {
            HUD.gameObject.SetActive(true);
            SpawnMenu.gameObject.SetActive(true);
            GraphicSettingMenu.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
            
        }

        void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Menu.gameObject.SetActive(!Menu.isActiveAndEnabled);
            }
        }
    }
}