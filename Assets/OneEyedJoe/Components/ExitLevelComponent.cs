using OneEyedJoe.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneEyedJoe.Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            FindObjectOfType<GameSession>().Save(); 
            SceneManager.LoadScene(_sceneName);
        }
        
    }
}
