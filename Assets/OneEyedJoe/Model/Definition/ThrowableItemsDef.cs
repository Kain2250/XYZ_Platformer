using System;
using UnityEngine;

namespace OneEyedJoe.Model.Definition
{
    [CreateAssetMenu(fileName = "Defs/ThrowableItemsDef", menuName = "ThrowableItemsDef")]
    public class ThrowableItemsDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items;
        
        public ThrowableDef Get(string id)
        {
            foreach (var throwableDef in _items)
            {
                if (throwableDef.Id == id)
                    return throwableDef;
            }

            return default;
        }
    }

    [Serializable]
    public struct ThrowableDef
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;
        
        public string Id => _id;
        public GameObject Projectile => _projectile;

        
        
        
    }
}