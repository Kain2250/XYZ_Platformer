using System;
using System.Collections.Generic;
using System.Linq;
using OneEyedJoe.Model.Definition;
using UnityEngine;

namespace OneEyedJoe.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();
        
        public delegate void OnInventoryChanged(string id, int countValue);
        public OnInventoryChanged OnChanged;
        
        public bool IsFull()
        {
            var sizeInventory = DefsFacade.I.Items.GetSizeInventory();

            if (_inventory != null)
                return _inventory.Count >= sizeInventory;
            return default;
        }

        public bool IsStacked(string id)
        {
            var itemDef = DefsFacade.I.Items.GetId(id);

            return itemDef.IsStacked;
        }

        public void Add(string id, int countValue)
        {
            
            if (countValue <= 0 ) return;

            var itemDef = DefsFacade.I.Items.GetId(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null || !itemDef.IsStacked)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            item.CountItem += countValue;
            
            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int countValue)
        {
            var itemDef = DefsFacade.I.Items.GetId(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null) return;

            item.CountItem -= countValue;
            
            if (item.CountItem <= 0)
                _inventory.Remove(item);
            
            OnChanged?.Invoke(id, Count(id));
        }

        public int GetValue(string id)
        {
            var itemDef = DefsFacade.I.Items.GetId(id);
            return itemDef.IsVoid ? 0 : itemDef.GetValue;
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }

            return null;
        }
        
        public int Count(string id)
        {
            var count = 0;

            foreach (var item in _inventory)
            {
                if (item.Id == id)
                    count = item.CountItem;
            }

            return count;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int CountItem;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
