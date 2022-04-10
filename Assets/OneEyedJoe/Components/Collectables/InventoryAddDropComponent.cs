using OneEyedJoe.Model;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Components.Collectables
{
    public class InventoryAddDropComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;
        
        private GameSession _session;

        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
        }
        
        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInventory>();
            var drop = go.GetInterface<ICanDropItem>();

            if (hero == null) return;
            
            var isFull = _session.Data.Inventory.IsFull();
            var itemIsPresent = _session.Data.Inventory.IsItemPresentInInventory(_id);
            var isStacked = _session.Data.Inventory.IsStacked(_id);

            if (itemIsPresent && isFull && isStacked)
            {
                hero.AddInInventory(_id, _count);
                return;
            }
            
            if (isFull)
                drop.Drop(gameObject);
            else
                hero.AddInInventory(_id, _count);
        }

    }
}