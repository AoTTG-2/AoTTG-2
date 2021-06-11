using Assets.Scripts.Characters.Humans;
using Assets.Scripts.UI.Input;
using Assets.Scripts.UI.Radial;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;
        private GameObject inventoryWheel;

        public GameObject RadialMenu;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<RadialMenu>(true).gameObject;
            inventoryWheel = gameObject.GetComponentInChildren<InventoryRadialMenu>(true).gameObject;
        }

        private void Update()
        {
            if (InputManager.KeyDown(InputUi.InteractionWheel))
            {
                if (!RadialMenu.activeSelf)
                {

                    DetermineRadialMenu().SetActive(true);

                    //RadialMenu.SetActive(true);
                    //interactionWheel.SetActive(true);
                }


            }

            if (InputManager.KeyUp(InputUi.InteractionWheel))
            {
                if (RadialMenu.activeSelf)
                    RadialMenu.SetActive(false);
            }

        }

        private GameObject DetermineRadialMenu()
        {

            RaycastHit hit;
            int layerMask = 1 << 25;

            var hero = Services.Service.Player.Self as Hero;

            Vector3 direction = Vector3.zero;

            direction = hero.transform.rotation * Vector3.forward;

            if (Physics.Raycast(hero.transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
            {

                Debug.DrawRay(hero.transform.position, transform.TransformDirection(direction) * hit.distance, Color.yellow, 10f);
                Debug.Log("Hit something");

                interactionWheel.SetActive(true);
                inventoryWheel.SetActive(false);
            }

            else
            {

                Debug.DrawRay(hero.transform.position, transform.TransformDirection(direction) * 1000, Color.blue, 10f);
                inventoryWheel.SetActive(true);
                interactionWheel.SetActive(false);
                
            }

            return RadialMenu;

        }

    }
}