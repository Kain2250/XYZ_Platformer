using OneEyedJoe.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneEyedJoe.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session);
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void ReloadWithDie()
        {
            FindObjectOfType<GameSession>().LoadLastSave();
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}

