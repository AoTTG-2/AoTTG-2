using Assets.Scripts.UI.Input;
using Assets.Scripts.UI.Radial;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;
        public GameObject[] hooksIndicators;

        public GameObject RadialMenu;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<RadialMenu>(true).gameObject;
            foreach(GameObject hook in hooksIndicators)
            {
                hook.transform.localScale = Vector3.one * PlayerPrefs.GetInt("Hook Indicator", 1);
            }
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

            if(InputManager.KeyDown(InputUi.HideHooks))
            {
                foreach(GameObject hook in hooksIndicators)
                {
                    if(hook.transform.localScale == Vector3.one)
                    {
                        hook.transform.localScale = Vector3.zero;
                        PlayerPrefs.SetInt("Hook Indicator", 0);
                    } else
                    {
                        hook.transform.localScale = Vector3.one;
                        PlayerPrefs.SetInt("Hook Indicator", 1);
                    }
                }
            }

        }
    }
}