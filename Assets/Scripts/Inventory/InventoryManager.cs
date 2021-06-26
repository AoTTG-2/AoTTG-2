using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;
using UnityEngine.Events;

namespace Assets.Scripts.Inventory
{

    public class InventoryManager : MonoBehaviour
    {

        public PlayerInventory BaseInventory;
        public Dictionary<Hero, PlayerInventory> playerInventories = new Dictionary<Hero, PlayerInventory>();
        public List<PlayerInventory> inventories = new List<PlayerInventory>();
        [HideInInspector] public UnityEvent<Hero> onInventoryChange;

        public void CreateNewInventory(Hero hero)
        {

            PlayerInventory newInv = ScriptableObject.CreateInstance<PlayerInventory>();

            playerInventories.Add(hero, newInv);

            foreach (InventoryItem item in BaseInventory.myInventory)
            {

                AddItemToInventory(hero, item);

            }

            //Uncomment to look at the inventory values

            ShowInventories();

        }

        public void ShowInventories()
        {

            inventories.Clear();

            foreach (PlayerInventory value in playerInventories.Values)
            {

                if (inventories.Contains(value))
                    continue;

                inventories.Add(value);

            }

        }

        public void AddItemToInventory(Hero hero, InventoryItem item)
        {

            try
            {

                PlayerInventory thisInventory = playerInventories[hero];

                thisInventory.myInventory.Add(item);

                onInventoryChange?.Invoke(hero);

            }

            catch (KeyNotFoundException)
            {

                Debug.LogError($"Could not find {hero}'s Inventory");

            }

        }

        public void RemoveItemFromInventory(Hero hero, InventoryItem item)
        {

            try
            {

                PlayerInventory thisInventory = playerInventories[hero];

                thisInventory.myInventory.Remove(item);

                onInventoryChange?.Invoke(hero);

            }

            catch (KeyNotFoundException)
            {

                Debug.LogError($"Could not find {hero}'s Inventory");

            }

        }

        public void RemovePlayerInventory(Hero hero)
        {

            playerInventories.Remove(hero);
            ShowInventories();

        }

        public void ClearAllInventories()
        {

            var keys = playerInventories.Keys;

            foreach(Hero hero in keys)
            {
                playerInventories.Remove(hero);
            }

        }

        void OnDestroy()
        {

            ClearAllInventories();

        }

    }

}
