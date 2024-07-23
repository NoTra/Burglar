using System;
using UnityEngine;
using System.Collections.Generic;
using EasyTransition;
using System.Collections;
using System.Reflection.Emit;

namespace burglar.managers
{
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager instance = null;

        public static LevelManager Instance => instance;
        
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
        }

        public void LoadScene(string sceneName)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundTransition);
            TransitionManager.Instance().Transition(sceneName, transition, 0f);

            var level = FindLevelBySceneName(sceneName);
            
            if (level == null)
            {
                Debug.LogError("Level not found for scene name : " + sceneName);
                return;
            }
            
            // Change current level index
            _currentLevelIndex = level.levelIndex;
            _currentLevel = level;
            
            Debug.Log("Level loaded : ");
            Debug.Log(level.levelName);
            Debug.Log(level.sceneName);
            Debug.Log(level.minimumCredits);
            Debug.Log(level.maximumCredits);

            // Trigger the event OnLoadLevel
            EventManager.OnLoadLevel();

            StartCoroutine(ShowHudAndHidePanelAfterTransition());
        }

        private LevelSO FindLevelBySceneName(string sceneName)
        {
            return levels.Find(level => level.sceneName == sceneName);
        }

        private IEnumerator ShowHudAndHidePanelAfterTransition()
        {
            yield return new WaitForSeconds(1.3f);

            // Deactivate UI Start Menu if opened
            // UIManager.Instance.StartScreen.SetActive(false);

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
            
            var levelSO = levels[_currentLevelIndex];
            // Load the next scene in Single mode
            LoadScene(levelSO.sceneName);
        }
    }
}
