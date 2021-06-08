using Assets.Scripts.Inventory.Items;
using Assets.Scripts.Inventory.Items.Data;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IInventoryService
    {
        IList<T> GetScriptableObjectItems<T>() where T : ItemData;
        IList<T> GetItems<T>() where T : Item;
    }
}
