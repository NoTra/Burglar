using System;
using UnityEngine;
using UnityEngine.Playables;

namespace burglar
{
    public class StartCinematic : MonoBehaviour
    {
        [SerializeField] PlayableDirector playableDirector;


        private void OnEnable()
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
        
        private void OnDisable()
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }

        private void Start()
        {
            if (playableDirector)
            {
                playableDirector.Play();
            }
        }

        private void OnPlayableDirectorStopped(PlayableDirector obj)
        {
            // Load main scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
        
        // If player skips the cinematic by pressing esc key
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("main");
            }
        }
    }
}
