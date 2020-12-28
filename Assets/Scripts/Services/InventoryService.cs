using Assets.Scripts.Items;
using Assets.Scripts.Items.Data;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Flare = Assets.Scripts.Items.Flare;

namespace Assets.Scripts.Services
{
    public class InventoryService : MonoBehaviour, IInventoryService
    {
        [SerializeField] private List<FlareData> flares;

        private List<Item> Inventory { get; } = new List<Item>();

        public IList<T> GetScriptableObjectItems<T>() where T : ItemData
        {
            var type = typeof(T);
            if (type == typeof(FlareData))
            {
                return flares.Cast<T>().ToList();
            }
            throw new NotImplementedException();
        }

        public IList<T> GetItems<T>() where T : Item
        {
            if (Inventory.Count == 0)
            {
                flares.ForEach(x => Inventory.Add(x.ToItem()));
            }

            var type = typeof(T);
            if (type == typeof(Flare))
            {
                return Inventory.OfType<Flare>().Cast<T>().ToList();
            }
            throw new NotImplementedException();
        }
    }
}
