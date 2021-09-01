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
        private GameObject inventoryWheel;

        public GameObject RadialMenu;
        public GameObject DebugMenu;

        public float interactionDistance = 10f;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<InteractRadialMenu>(true).gameObject;
            inventoryWheel = gameObject.GetComponentInChildren<InventoryRadialMenu>(true).gameObject;
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

                    DetermineRadialMenu().SetActive(true);

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

            int layerMask = 1 << 25; //Item Layer

            var camera = GameObject.Find("MainCamera").GetComponent<UnityEngine.Camera>();

            Ray ray = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && hit.distance < interactionDistance)
            {
                interactionWheel.GetComponent<InteractRadialMenu>().targetObject = hit.collider.gameObject;
                interactionWheel.SetActive(true);
                inventoryWheel.SetActive(false);
            }

            else
            {
                inventoryWheel.SetActive(true);
                interactionWheel.SetActive(false);
            }

            return RadialMenu;

        }

    }
}