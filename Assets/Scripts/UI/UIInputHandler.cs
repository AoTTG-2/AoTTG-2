using Assets.Scripts.UI.Input;
using Assets.Scripts.UI.Radial;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// A component attached to the Canvas GameObject, which controls the Radial Menu
    /// </summary>
    public class UIInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;

        public GameObject RadialMenu;
        public GameObject DebugMenu;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<RadialMenu>(true).gameObject;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F7))
            {
                DebugMenu.SetActive(!DebugMenu.activeSelf);
            }
            
            if (InputManager.KeyDown(InputUi.InteractionWheel))
            {
                if (!RadialMenu.activeSelf)
                {
                    RadialMenu.SetActive(true);
                    interactionWheel.SetActive(true);
                }


            }

            if (InputManager.KeyUp(InputUi.InteractionWheel))
            {
                if (RadialMenu.activeSelf)
                    RadialMenu.SetActive(false);
            }

        }
    }
}