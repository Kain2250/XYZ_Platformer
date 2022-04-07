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

        public bool IsItemPresentInInventory(string id)
        {
            var isPresent = _inventory.Contains(GetItem(id));
            return isPresent;
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

            if (itemDef.IsStacked)
                AddStackItem(id, countValue);
            else
                AddNonStackItem(id, countValue);

            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int countValue)
        {
            var itemDef = DefsFacade.I.Items.GetId(id);
            if (itemDef.IsVoid) return;

            if (itemDef.IsStacked)
                RemoveStackItem(id, countValue);
            else
                RemoveNonStackItem(id, countValue);
            
            OnChanged?.Invoke(id, Count(id));
        }
        
        private void AddStackItem(string id, int countValue)
        {
            var item = GetItem(id);
            if (item == null)
            {
                if (IsFull()) return;

                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            item.CountItem += countValue;
        }
        
        private void AddNonStackItem(string id, int countValue)
        {
            for (int i = 0; i < countValue; i++)
            {
                if (IsFull()) return;

                var item = new InventoryItemData(id) {CountItem = 1};
                _inventory.Add(item);
            }
        }


        private void RemoveStackItem(string id, int countValue)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.CountItem -= countValue;
            
            if (item.CountItem <= 0)
                _inventory.Remove(item);
        }
        private void RemoveNonStackItem(string id, int countValue)
        {
            for (int i = 0; i < countValue; i++)
            {
                var item = GetItem(id);
                if (item == null) return;

                _inventory.Remove(item);
            }
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
