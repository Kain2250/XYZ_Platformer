using OneEyedJoe.Model;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Model.Definition;
using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;
        
        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var areAllRequirementsMet = true;
            foreach (var item in _required)
            {
                var numItems = session.Data.Inventory.Count(item.Id);
                if (numItems < item.CountItem)
                    areAllRequirementsMet = false;
            }
            
            if (areAllRequirementsMet)
            {
                if (_removeAfterUse)
                {
                    foreach (var item in _required)
                        session.Data.Inventory.Remove(item.Id, item.CountItem);
                }
                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}