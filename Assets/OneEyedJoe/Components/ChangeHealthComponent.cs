using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneEyedJoe.Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _changeHealthValue;

        private StatsPlayerComponent _statsPlayerComponent;

        public void ApplyDamage(GameObject target)
        {
            _statsPlayerComponent = target.GetComponent<StatsPlayerComponent>();

            if (_statsPlayerComponent != null)
            {
                _statsPlayerComponent.ApplyDammge(_changeHealthValue);
            }
        }

        public void ApplyHeal(GameObject target)
        {
            _statsPlayerComponent = target.GetComponent<StatsPlayerComponent>();

            if (_statsPlayerComponent != null)
            {
                _statsPlayerComponent.ApplyHeal(_changeHealthValue);
            }
        }

    }

}
