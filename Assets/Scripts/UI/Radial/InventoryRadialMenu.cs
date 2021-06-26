using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Inventory;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;

namespace Assets.Scripts.UI.Radial
{
    public class InventoryRadialMenu : RadialMenu
    {

        InventoryManager inventoryManager;
        private Hero thisHero;
        private void OnEnable()
        {

            MenuManager.RegisterOpened(this);
            Cursor.visible = false;
            thisHero = Service.Player.Self as Hero;
            PopulateWheel(thisHero);

        }

        protected override void Start()
        {

            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            inventoryManager.onInventoryChange.AddListener(SyncWheel);
            thisHero = Service.Player.Self as Hero;
            PopulateWheel(thisHero);
            Label.text = "Inventory";

        }

        protected void PopulateWheel(Hero hero)
        {

            if (hero == null) return;

            var playerInventory = inventoryManager.playerInventories[hero].myInventory;

            foreach (InventoryItem item in playerInventory)
            {

                if (item == null || itemMenus.Contains(item.thisElement))
                    continue;

                if (item.itemMenu != null)
                {

                    RadialElement temp = Instantiate(RadialElementPrefab, transform);
                    temp.NextMenu = item.itemMenu;
                    temp.IconText.text = item.itemName;
                    temp.thisItem = item.thisItem;
                    item.thisElement = temp;
                    temp.thisInventoryItem = item;
                    temp.PreviousMenu = this;
                    itemMenus.Add(temp);

                }

                else
                {

                    RadialElement temp = Instantiate(RadialElementPrefab, transform);
                    temp.thisItem = item.thisItem;
                    temp.IconText.text = item.itemName;
                    item.thisElement = temp;
                    temp.thisInventoryItem = item;
                    itemMenus.Add(temp);

                }

            }

            Pieces = itemMenus.ToArray();
            StartCoroutine(SpawnButtons());

        }

        protected void SyncWheel(Hero hero)
        {

            CleanWheel(hero);

        }

        protected void CleanWheel(Hero hero)
        {

            var playerInventory = inventoryManager.playerInventories[hero].myInventory;

            foreach (RadialElement element in itemMenus.ToArray())
            {

                if (element.thisInventoryItem == null)
                    continue;

                if (!playerInventory.Contains(element.thisInventoryItem))
                {

                    itemMenus.Remove(element);
                    Destroy(element.gameObject);

                }

            }

        }

    }

}
