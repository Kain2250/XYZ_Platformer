using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OneEyedJoe.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        
        public void Apply(int changeHealthValue)
        {
            _health += changeHealthValue;
            switch (changeHealthValue >= 0)
            {
                case true:
                    _onHeal?.Invoke();
                    break;
                default:
                    switch (_health <= 0)
                    {
                        case true:
                            _onDie?.Invoke();
                            break;
                        default:
                            _onDamage?.Invoke();
                            break;
                    }
                    break;
            }
        }
    }
}
