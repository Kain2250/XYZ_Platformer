using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OneEyedJoe.Components
{
    public class StatsPlayerComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _mana;
        [SerializeField] private UnityEvent _onDammage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onManaRegen;

        public void ApplyDammge(int dammageValue)
        {
            _health -= dammageValue;
            _onDammage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void ApplyHeal(int healValue)
        {
            _health += healValue;
            _onHeal?.Invoke();
            Debug.Log($"Heal Player - {healValue}. Health = {_health}");

        }

        public void ApplyMana(int manaValue)
        {
            _mana += manaValue;
            _onManaRegen?.Invoke();
            Debug.Log($"Mana regeneration on {manaValue} point. Mana = {_mana}");
        }
    }
}
