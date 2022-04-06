using System;
using System.Collections.Generic;
using UnityEngine;

namespace OneEyedJoe.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private int _sizeInventory = 6;
        [SerializeField] private ItemDef[] _items;
        
        public int GetSizeInventory()
        {
            return _sizeInventory;
        }

        public ItemDef GetId(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
        
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif
        
    }
    
    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _isStacked;
        [SerializeField] private int _value;
        
        public string Id => _id;
        public bool IsVoid => string.IsNullOrEmpty(_id);
        
        public bool IsStacked => _isStacked;
        public int GetValue => _value;
    }
}