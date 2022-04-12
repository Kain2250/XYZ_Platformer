using System;
using System.Linq;
using UnityEngine;

namespace OneEyedJoe.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;
        
        public ItemDef Get(string id)
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
        [SerializeField] private ItemTag[] _tags;
        //[SerializeField] private bool _isStacked;
        [SerializeField] private int _value;
        [SerializeField] private Sprite _icon;
        
        public string Id => _id;
        public bool IsVoid => string.IsNullOrEmpty(_id);
        
        //public bool IsStacked => _isStacked;
        public int GetValue => _value;
        public Sprite Icon => _icon;

        public bool HasTag(ItemTag tag)
        {
            return _tags.Contains(tag);
        }
    }
}