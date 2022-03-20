using OneEyedJoe.Model;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.UI.Widgets;
using UnityEngine;

namespace OneEyedJoe.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private CoinWidget _coinBar;
        [SerializeField] private WeaponWidget _weaponBar;
        
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;
            _session.Data.Coin.OnChanged += OnCoinChanged;
            _session.Data.Weapon.OnChanged += OnWeaponChanged;
            
            OnCoinChanged(_session.Data.Coin.Value, 0);
            
            OnWeaponChanged(_session.Data.Weapon.Value, 0);
            
            OnHealthChanged(_session.Data.Hp.Value, 1);
        }
        private void OnCoinChanged(int newvalue, int oldvalue)
        {
            _coinBar.SetCoinCount(newvalue);
        }
        
        private void OnWeaponChanged(int newvalue, int oldvalue)
        {
            _weaponBar.SetWeaponCount(newvalue);
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
            _session.Data.Coin.OnChanged -= OnCoinChanged;
            _session.Data.Weapon.OnChanged -= OnWeaponChanged;
        }
    }
}
