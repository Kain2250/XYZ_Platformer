using OneEyedJoe.Model;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;
        
        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInventory>();
            hero?.AddInInventory(_id, _count);
        }
    }
}