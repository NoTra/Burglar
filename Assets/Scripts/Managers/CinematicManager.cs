using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace burglar.managers
{
    public class CinematicManager : MonoBehaviour
    {
        // Singleton
        private static CinematicManager instance = null;
        
        public static CinematicManager Instance => instance;

        private PlayableDirector _currentScenePlayableDirector;
        
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

        private void OnEnable()
        {
            EventManager.LoadLevelEnd += LaunchCinematic;
        }

        public void LaunchCinematic([CanBeNull] PlayableAsset cinematic = null)
        {
            Debug.Log("[CinematicManager] LaunchCinematic(cinematic)");
            
            if (cinematic)
            {
                Debug.Log("[CinematicManager] LaunchCinematic(cinematic) -> PlayCinematic() : " + _currentScenePlayableDirector.playableAsset.name);
                StartCoroutine(PlayCinematic(cinematic as TimelineAsset));
                
                return;
            }
        }
        
        private void LaunchCinematic()
        {
            Debug.Log("[CinematicManager] LaunchCinematic()");
            var currentLevel = LevelManager.Instance._currentLevel;
            
            if (currentLevel.startCinematic)
            {
                Debug.Log("[CinematicManager] LaunchCinematic() -> PlayCinematic()");
                StartCoroutine(PlayCinematic(currentLevel.startCinematic));
            }
        }
        
        private IEnumerator PlayCinematic(TimelineAsset cinematic)
        {
            EventManager.OnCinematicStart();
            
            Debug.Log("Play(" + cinematic.name + ")");
            
            _currentScenePlayableDirector.playableAsset = cinematic;
            _currentScenePlayableDirector.Play();
            yield return new WaitForSeconds((float)_currentScenePlayableDirector.duration);
            _currentScenePlayableDirector.Stop();
            
            EventManager.OnCinematicEnd();
        }
        
        public void SetCurrentScenePlayableDirector(PlayableDirector playableDirector)
        {
            _currentScenePlayableDirector = playableDirector;
        }
    }
}
