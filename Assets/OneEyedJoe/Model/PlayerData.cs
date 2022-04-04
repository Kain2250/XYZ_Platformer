using System;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Data.Properties;
using UnityEngine;

namespace OneEyedJoe.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public IntProperty Hp = new IntProperty();
       //public IntProperty Coin = new IntProperty();
       //public IntProperty Weapon = new IntProperty();
        
        public InventoryData Inventory => _inventory;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}
