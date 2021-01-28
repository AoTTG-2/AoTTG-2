using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<InteractionWheel>(true).gameObject;
        }

        private void Update()
        {
            if (InputManager.KeyDown(InputUi.InteractionWheel))
            {
                if (!interactionWheel.activeSelf)
                    interactionWheel.SetActive(true);
            }

            if (InputManager.KeyUp(InputUi.InteractionWheel))
            {
                if (interactionWheel.activeSelf)
                    interactionWheel.SetActive(false);
            }

        }
    }
}