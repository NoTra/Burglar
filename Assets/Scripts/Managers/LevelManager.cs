using UnityEngine;
using System.Collections.Generic;
using EasyTransition;
using System.Collections;
using UnityEngine.SceneManagement;


namespace burglar.managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager instance = null;

        public static LevelManager Instance => instance;
        
        // List of scenes
        public List<string> levels;

        // Current level index
        private int _currentLevelIndex = 0;

        // [SerializeField] private EasyTransition.Transition _transition;
        [SerializeField] private TransitionSettings transition;

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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundTransition);
            TransitionManager.Instance().Transition(sceneName, transition, 0f);

            StartCoroutine(ShowHudAndHidePanelAfterTransition());
        }

        private IEnumerator ShowHudAndHidePanelAfterTransition()
        {
            yield return new WaitForSeconds(1.3f);

            // Deactivate UI Start Menu if opened
            UIManager.Instance.StartScreen.SetActive(false);

            // Activate the game HUD
            UIManager.Instance.HUD.SetActive(true);
        }

        public void LoadShop()
        {
            // Change music for shop music
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicShop, false);
            
            LoadScene("shop");
        }

        public void LoadNextLevel()
        {
            _currentLevelIndex++;

            Debug.Log("Change music to shop");
            
            // Change music for level music
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicTuto, false);
            
            // Load the next scene in Single mode
            LoadScene(levels[_currentLevelIndex]);
        }
    }
}
