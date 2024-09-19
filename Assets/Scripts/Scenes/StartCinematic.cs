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
            CinematicManager.Instance.ClearCurrentCinematic();
            LevelManager.Instance.LoadScene("intro");
            // UnityEngine.SceneManagement.SceneManager.LoadScene("intro");
        }
    }
}
