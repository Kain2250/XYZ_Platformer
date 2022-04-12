using OneEyedJoe.Model;
using OneEyedJoe.Model.Definition;
using OneEyedJoe.UI.Widgets;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;

namespace OneEyedJoe.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private CoinWidget _coinBar;
        
        private GameSession _session;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
        }
        
        private void Start()
        {
            _trash.Retain(_session.Data.Hp.Subscribe(OnHealthChanged));
            _trash.Retain(_session.Data.Inventory.Subscribe(OnChangedInventory));
            
            OnHealthChanged(_session.Data.Hp.Value, 1);
            OnChangedInventory("Coin", _session.Data.Inventory.Count("Coin"));
        }
        
        private void OnChangedInventory(string id, int value)
        {
            if (id == "Coin") _coinBar.SetCoinCount(value);
        }
        
        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
