using Assets.Scripts.Items;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IInventoryService
    {
        IList<T> GetScriptableObjectItems<T>() where T : Item;
        IList<T> GetItems<T>() where T : Item;
    }
}
