using System;
using burglar.managers;
using UnityEngine;
using UnityEngine.Playables;

namespace burglar
{
    public class StartCinematic : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.CinematicEnd += LoadIntro;
            // EventManager.LoadLevelStart += LoadCinematic;
        }

        private void LoadCinematic()
        {
            var currentLevel = LevelManager.Instance._currentLevel;
            if (currentLevel.startCinematic)
            {
                Debug.Log("LaunchCinematic with startCinematic");
                CinematicManager.Instance.LaunchCinematic(currentLevel.startCinematic);
            }
        }

        private void OnDisable()
        {
            EventManager.CinematicEnd -= LoadIntro;
        }

        // If player skips the cinematic by pressing esc key
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadIntro();
            }
        }

        private void LoadIntro()
        {
            LevelManager.Instance.LoadScene("intro");
            // UnityEngine.SceneManagement.SceneManager.LoadScene("intro");
        }
    }
}
