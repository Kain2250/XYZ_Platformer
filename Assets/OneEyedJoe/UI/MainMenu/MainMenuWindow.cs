using System;
using OneEyedJoe.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneEyedJoe.UI.MainMenu
{
    public class MainMenuWindow : AnimateWindow
    {
        private Action _closeAction;
        
        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("UI/Settings");
            
            var canvas = FindObjectsOfType<Canvas>();
            
            Instantiate(window, canvas[canvas.Length > 1 ? 1 : 0].transform);
        }

        public void RestartLevel()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session);
            
            var scene = SceneManager.GetActiveScene();
            Time.timeScale = 1f;
            SceneManager.LoadScene(scene.name);
        }
        
        public void OnStartGames()
        {
            _closeAction = () =>
            {
                SceneManager.LoadScene("Level1");
            };
            Close();
        }
        
        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();
            
        }
    }
}
