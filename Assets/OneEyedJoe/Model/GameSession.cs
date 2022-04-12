using System;
using OneEyedJoe.Model.Data;
using OneEyedJoe.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneEyedJoe.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        
        private bool _isPause;
        private PlayerData _save;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        
        public bool IsPause
        {
            get => _isPause;
            set => _isPause = value;
        }

        private void Awake()
        {
            _isPause = false;
            
            LoadHud();

            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);
            _trash.Retain(QuickInventory);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }

            return false;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _isPause = false;
            _data = _save.Clone();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
