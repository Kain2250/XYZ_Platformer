using OneEyedJoe.Model;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.Utils;
using UnityEngine;

namespace OneEyedJoe.Components.Collectables
{
    public class InventoryDropComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;

        public void Drop(GameObject go)
        {
            var drop = go.GetInterface<ICanDropItem>();
            drop?.Drop(gameObject);
        }
 
    }
}