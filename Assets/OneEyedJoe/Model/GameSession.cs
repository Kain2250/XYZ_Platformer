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
                DontDestroyOnLoad(this);
            }
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
    }
}
