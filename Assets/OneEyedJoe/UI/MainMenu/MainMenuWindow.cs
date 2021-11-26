using System;
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
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
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
