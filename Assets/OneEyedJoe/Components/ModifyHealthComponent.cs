using UnityEngine;

namespace OneEyedJoe.Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _changeHealthValue;

        private HealthComponent _healthComponent;
        
        public void Apply(GameObject target)
        {
            _healthComponent = target.GetComponent<HealthComponent>();

            if (_healthComponent != null)
            {
                _healthComponent.Apply(_changeHealthValue);
            }
        }
    }
}
