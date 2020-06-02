using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : MonoBehaviour
    {
        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenu SpawnMenu;

        void OnEnable()
        {
            HUD.gameObject.SetActive(true);
            SpawnMenu.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
        }

        void Update()
        {
            // The Escape key unlocks the cursor in the editor,
            // which is why exiting the menu messes with TPS.
            if (Input.GetKeyDown(KeyCode.P))
                Menu.gameObject.SetActive(!Menu.isActiveAndEnabled);
        }
    }
}