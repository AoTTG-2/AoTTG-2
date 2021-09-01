using UnityEngine;
using Assets.Scripts.Inventory;
using Assets.Scripts.Characters.Humans;

namespace Assets.Scripts.UI.Radial
{
    public class InteractRadialMenu : RadialMenu
    {
        public GameObject targetObject;
        private InventoryManager inventoryManager;

        protected override void Start()
        {
            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            Label.text = "Interact";

            var PickUp = Instantiate(RadialElementPrefab, transform);
            var Examine = Instantiate(RadialElementPrefab, transform);

            PickUp.IconText.text = "Pick Up Item";
            Examine.IconText.text = "Examine Item";

            Pieces = new[]
            {

                PickUp,
                Examine

            };

            StartCoroutine(SpawnButtons());
        }

        protected override void OnElementClicked(RadialElement element, int index)
        {
            var item = targetObject.GetComponent<PhysicalItem>();
            switch (index)
            {
                case 0:
                    var hero = Services.Service.Player.Self as Hero;
                    inventoryManager.AddItemToInventory(hero, item.myInventoryItem);
                    Destroy(item.gameObject);
                    return;
                case 1:
                    Debug.Log(item.myInventoryItem.itemDesc); //TODO Logic for displaying the item desciption
                    return;
                default:
                    Debug.LogError("[Interact Radial]: You pressed the nonexistant third button.");
                    return;
            }

        }

    }

}
