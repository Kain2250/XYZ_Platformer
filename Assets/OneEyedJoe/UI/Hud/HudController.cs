using System;
using OneEyedJoe.Model;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.UI.MainMenu;
using OneEyedJoe.UI.Widgets;
using UnityEngine;

namespace OneEyedJoe.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private CoinWidget _coinBar;
        [SerializeField] private WeaponWidget _weaponBar;
        [SerializeField] private PotionWidget _potionBar;
        
        private GameSession _session;
        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
        }
        
        private void Start()
        {
            _session.Data.Hp.OnChanged += OnHealthChanged;
            _session.Data.Inventory.OnChanged += OnChangedInventory;
            
            OnHealthChanged(_session.Data.Hp.Value, 1);
            OnChangedInventory("Sword", _session.Data.Inventory.Count("Sword"));
            OnChangedInventory("Coin", _session.Data.Inventory.Count("Coin"));
            OnChangedInventory("Potion", _session.Data.Inventory.Count("Potion"));
        }
        
        private void OnChangedInventory(string id, int value)
        {
            switch (id)
            {
                case "Sword":
                    _weaponBar.SetWeaponCount(value);
                    break;
                case "Coin":
                    _coinBar.SetCoinCount(value);
                    break;
                case "Potion":
                    var potionValue = _session.Data.Inventory.CountAll("Potion");
                    _potionBar.SetPotionCount(potionValue);
                    break;
            }
        }
        
        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
            _session.Data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}
