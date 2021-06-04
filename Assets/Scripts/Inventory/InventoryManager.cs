using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Inventory
{

    public class InventoryManager : MonoBehaviour
    {

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public PlayerInventory playerInventory;

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))  //Hard coded to test functionality
            {

                playerInventory.myInventory[0].Use();

            }

        }

    }

}