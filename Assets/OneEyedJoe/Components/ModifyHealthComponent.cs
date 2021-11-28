using UnityEngine;

namespace OneEyedJoe.Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _changeHealthValue;
        
        public void Apply(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.Apply(_changeHealthValue);
            }
        }
    }
}
