using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;
        public GameObject[] hooksIndicators;

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

            if(InputManager.KeyDown(InputUi.HideHooks))
            {
                foreach(GameObject hook in hooksIndicators)
                {
                    if(hook.activeSelf)
                    {
                        hook.SetActive(false);
                    } else
                    {
                        hook.SetActive(true);
                    }
                }
            }

        }
    }
}