using System;
using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private HealthChangeEvent _onChange;

        public void Apply(int changeHealthValue)
        {
            if (_health <= 0) return;
            
            _health += changeHealthValue;
            _onChange?.Invoke(_health);
            
            if (changeHealthValue < 0)
                _onDamage?.Invoke();
            if (_health > 0)
                _onHeal?.Invoke();
            if (_health <= 0)
                _onDie?.Invoke();
        }
        
#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif

        public void SetHealth(int health)
        {
            _health = health;
        }
        
        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
            
        }

    }
}
