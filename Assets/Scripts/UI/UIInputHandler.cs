using Assets.Scripts.UI.Input;
using Assets.Scripts.UI.Radial;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;

        public GameObject RadialMenu;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<RadialMenu>(true).gameObject;
        }

        private void Update()
        {
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