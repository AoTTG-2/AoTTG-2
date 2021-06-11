using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;

namespace Assets.Scripts.Inventory
{
    public class PhysicalItem : MonoBehaviour
    {

        public InventoryItem myInventoryItem;
        private InventoryManager inventoryManager;

        private void Start()
        {
            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            gameObject.tag = "item";
            gameObject.layer = 25; //25 is current item layer
        }

        private void OnCollisionEnter(Collision other)
        {

            if (other.collider.CompareTag("Player"))
            {

                Hero hero = other.collider.gameObject.GetComponent<Hero>();

                inventoryManager.AddItemToInventory(hero, myInventoryItem);

                Destroy(this.gameObject);

            }

        }

    }

}
