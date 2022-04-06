using OneEyedJoe.Creatures.Hero;
using OneEyedJoe.Model;
using OneEyedJoe.Model.Definition;
using UnityEngine;

namespace OneEyedJoe.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
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
            var hero = go.GetComponent<Hero>();

            if (hero == null) return;
            
            var isFull = _session.Data.Inventory.IsFull();
            var isStacked = _session.Data.Inventory.IsStacked(_id);
                
            if (isFull && !isStacked)
            {
                hero.Drop(gameObject);
            }
            else
            {
                hero.AddInInventory(_id, _count);
            }
        }
    }
}