using System;
using System.Collections.Generic;
using System.Linq;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;

namespace OneEyedJoe.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();
        
        public delegate void OnInventoryChanged(string id, int countValue);
        public OnInventoryChanged OnChanged;
        
        public IDisposable Subscribe(OnInventoryChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        
        public bool IsFull()
        {
            var sizeInventory = DefsFacade.I.Player.SizeInventory;

            if (sizeInventory == 0) return false;

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
            var itemDef = DefsFacade.I.Items.Get(id);

            return itemDef.HasTag(ItemTag.Stackable);
        }

        public void Add(string id, int countValue)
        {
            if (countValue <= 0 ) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.HasTag(ItemTag.Stackable))
                AddStackItem(id, countValue);
            else
                AddNonStackItem(id, countValue);

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
        
        public void Remove(string id, int countValue)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.HasTag(ItemTag.Stackable))
                RemoveStackItem(id, countValue);
            else
                RemoveNonStackItem(id, countValue);
            
            OnChanged?.Invoke(id, Count(id));
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
            var itemDef = DefsFacade.I.Items.Get(id);
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

        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();
            
            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));
                if (isAllRequirementsMet)
                    retValue.Add(item);
            }
            
            return retValue.ToArray();
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

        public int CountAll(string id)
        {
            var count = 0;
            
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    count++;
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
