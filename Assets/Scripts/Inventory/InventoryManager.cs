using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;
using UnityEngine.Events;

namespace Assets.Scripts.Inventory
{
    /// <summary>
    /// Manages all inventories in the current session.
    /// Contains a dict using Hero and Playerinventory as key/value pair which means inventory resets on death.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        public PlayerInventory BaseInventory;
        public Dictionary<Hero, PlayerInventory> playerInventories = new Dictionary<Hero, PlayerInventory>();
#if UNITY_EDITOR
        public List<PlayerInventory> inventories = new List<PlayerInventory>(); //Helpful for debugging since dicts dont get serialized
#endif
        [HideInInspector] public UnityEvent<Hero> onInventoryChange;
        public void CreateNewInventory(Hero hero)
        {
            PlayerInventory newInv = ScriptableObject.CreateInstance<PlayerInventory>();
            playerInventories.Add(hero, newInv);

            foreach (InventoryItem item in BaseInventory.myInventory)
            {
                AddItemToInventory(hero, item);
            }
#if UNITY_EDITOR
            ShowInventories();
#endif
        }

        /// <summary>
        /// Debug function to populate the list since Unity doesn't serialize dicts so we need a workaround.
        /// </summary>
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

        /// <summary>
        /// Adds item to hero's inventory.
        /// </summary>
        /// <param name="hero">The hero who's inventory we are altering.</param>
        /// <param name="item">The item we are adding to the inventory.</param>
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

        /// <summary>
        /// Remove item from hero's inventory.
        /// </summary>
        /// <param name="hero">The hero who's inventory we are altering.</param>
        /// <param name="item">The item we are removing from the inventory.</param>
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

        /// <summary>
        /// Delete a hero's inventory.
        /// </summary>
        /// <param name="hero">The hero who's inventory we are deleting</param>
        public void RemovePlayerInventory(Hero hero)
        {
            playerInventories.Remove(hero);
        }

        /// <summary>
        /// Cleanup function for clearing the dictionary.
        /// </summary>
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
