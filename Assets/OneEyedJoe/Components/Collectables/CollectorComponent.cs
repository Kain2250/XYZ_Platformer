using System.Collections.Generic;
using OneEyedJoe.Model;
using OneEyedJoe.Model.Data;
using UnityEngine;

namespace OneEyedJoe.Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();
        public void AddInInventory(string id, int value)
        {
            _items.Add(new InventoryItemData(id){CountItem = value});
        }

        public void DropInInventory()
        {
            var session = FindObjectOfType<GameSession>();
            foreach (var inventoryItemData in _items)
            {
                session.Data.Inventory.Add(inventoryItemData.Id, inventoryItemData.CountItem);
            }
        }
    }
}