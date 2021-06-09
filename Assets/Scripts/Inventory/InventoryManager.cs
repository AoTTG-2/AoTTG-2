using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;
using UnityEngine.Events;
using Assets.Scripts.UI.Radial;

namespace Assets.Scripts.Inventory
{

    public class InventoryManager : MonoBehaviour
    {

        public PlayerInventory BaseInventory;
        public Dictionary<Hero, PlayerInventory> playerInventories = new Dictionary<Hero, PlayerInventory>();
        public List<PlayerInventory> inventories = new List<PlayerInventory>();

        public void CreateNewInventory(Hero hero)
        {

            PlayerInventory newInv = ScriptableObject.CreateInstance<PlayerInventory>();

            playerInventories.Add(hero, newInv);

            foreach (InventoryItem item in BaseInventory.myInventory)
            {

                AddItemToInventory(hero, item);

            }

            //Uncomment to look at the inventory values
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

            }

            catch (KeyNotFoundException)
            {

                Debug.LogError($"Could not find {hero}'s Inventory");

            }

        }

    }

}
