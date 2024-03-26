using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


namespace burglar
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager instance = null;

        public static LevelManager Instance => instance;

        // List of scenes
        public List<string> levels;

        // Current level index
        private int _currentLevelIndex = 0;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            // @TODO : create scene transition

            // Deactivate UI Start Menu if opened
            UIManager.Instance.StartScreen.SetActive(false);
            // Activate the game HUD
            UIManager.Instance.HUD.SetActive(true);

            // Load the scene in Single mode
            SceneManager.LoadScene(sceneName);
        }

        public void LoadShop()
        {
            SceneManager.LoadScene("shop");
        }

        public void LoadNextLevel()
        {
            // @TODO : create scene transition

            _currentLevelIndex++;

            // Load the next scene in Single mode
            SceneManager.LoadScene(levels[_currentLevelIndex]);
        }
    }
}
