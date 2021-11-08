 using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe.Components
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