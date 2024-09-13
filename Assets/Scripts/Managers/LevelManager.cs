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

        private Coroutine playerCaughtCoroutine;
        
        // List of scenes
        public List<LevelSO> levels;

        private List<string> _levelNames = new List<string>();

        // Current level index
        public int _currentLevelIndex = 0;
        
        public LevelSO _currentLevel;

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

        private void Start()
        {
            // Loop through the levels and extract the names in levelNames List<string>
            foreach (var level in levels)
            {
                _levelNames.Add(level.levelName);
            }
            
            // Get current sceneName
            var currentScene = SceneManager.GetActiveScene();
            
            // Find the current level
            _currentLevel = FindLevelBySceneName(currentScene.name);
            
            if (_currentLevel != null)
            {
                _currentLevelIndex = _currentLevel.levelIndex;
            }
        }

        public void LoadLevelByIndex(int index)
        {
            if (index < 0 || index >= levels.Count)
            {
                Debug.LogError("Level index out of range");
                return;
            }
            
            _currentLevelIndex = index;
            _currentLevel = levels[_currentLevelIndex];
        }

        public void LoadScene(string sceneName)
        {
            // Play transition (effect & sound)
            StartCoroutine(PlayTransitionAndLoadLevel(sceneName));
        }

        private IEnumerator PlayTransitionAndLoadLevel(string sceneName)
        {
            var level = FindLevelBySceneName(sceneName);
            
            if (!level && sceneName != "shop" && sceneName != "main")
            {
                Debug.LogError("Level not found for scene name : " + sceneName);
                yield break;
            }
            
            // Change current level index
            _currentLevelIndex = level.levelIndex;
            _currentLevel = level;
            
            // Trigger the event OnLoadLevel
            EventManager.OnLoadLevelStart();
            
            yield return StartCoroutine(PlayTransition(sceneName));
            
            EventManager.OnLoadLevelEnd();
            
            // Clear previous level
            
        }

        private IEnumerator PlayTransition(string sceneName)
        {
            AudioManager.Instance.StopMusic();
            
            TransitionManager.Instance().SetRunningTransition(false);
            TransitionManager.Instance().Transition(sceneName, UIManager.Instance.levelTransition, 0f);
            
            yield return null;
        }

        private LevelSO FindLevelBySceneName(string sceneName)
        {
            return levels.Find(level => level.sceneName == sceneName);
        }
        
        public void LoadShop()
        {
            // Change music for shop music
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicShop, true);
            
            // Trigger the event OnLoadLevel
            // EventManager.OnLoadLevelStart();
            
            StartCoroutine(PlayTransition("shop"));

            // EventManager.OnLoadLevelEnd();
        }
        
        public void LoadEndCredits()
        {
            // Trigger the event OnLoadLevel
            // EventManager.OnLoadLevelStart();
            
            AudioManager.Instance.StopMusic();
            
            StartCoroutine(PlayTransition("EndCredit"));
            
            // EventManager.OnLoadLevelEnd();
        }
        
        public void LoadMainMenu()
        {
            // Change music for shop music
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicMenu, true);
            
            // Trigger the event OnLoadLevel
            // EventManager.OnLoadLevelStart();
            
            StartCoroutine(PlayTransition("main"));
            
            // EventManager.OnLoadLevelEnd();
        }

        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            
            if (_currentLevelIndex >= levels.Count)
            {
                Debug.Log("No more levels");
                return;
            }
            
            var levelSO = levels[_currentLevelIndex];
            
            // Load the next scene in Single mode
            LoadScene(levelSO.sceneName);
        }
    }
}
