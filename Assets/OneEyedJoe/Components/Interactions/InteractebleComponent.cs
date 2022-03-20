using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe.Components.Interactions
{
    public class InteractebleComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}