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
        
        [CanBeNull] private Coroutine _currentCinematicCoroutine = null;
        
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
        
        private void OnDisable()
        {
            EventManager.LoadLevelEnd -= LaunchCinematic;
        }

        public void LaunchCinematic([CanBeNull] PlayableAsset cinematic = null)
        {
            // if (_currentCinematicCoroutine != null)
            // {
            //     Debug.Log("Cinematic already playing (blocked " + cinematic.name + ")");
            //     return;
            // }
            
            if (cinematic)
            {
                _currentCinematicCoroutine = StartCoroutine(PlayCinematic(cinematic as TimelineAsset));
                
                return;
            }
        }
        
        private void LaunchCinematic()
        {
            if (_currentCinematicCoroutine != null)
            {
                Debug.Log("Cinematic already playing");
                return;
            }
            
            var currentLevel = LevelManager.Instance._currentLevel;
            
            if (currentLevel.startCinematic)
            {
                Debug.Log("LaunchCinematic(" + currentLevel.startCinematic.name + " for " + currentLevel.startCinematic.duration + "s)");
                _currentCinematicCoroutine = StartCoroutine(PlayCinematic(currentLevel.startCinematic));
            }
        }
        
        private IEnumerator PlayCinematic(TimelineAsset cinematic)
        {
            EventManager.OnCinematicStart();
            
            if (_currentScenePlayableDirector == null)
            {
                Debug.LogError("No PlayableDirector set for CinematicManager");
                yield break;
            }
            
            _currentScenePlayableDirector.playableAsset = cinematic;
            _currentScenePlayableDirector?.Play();
            yield return new WaitForSeconds((float)_currentScenePlayableDirector.duration);
            
            if (_currentScenePlayableDirector != null)
            {
                _currentScenePlayableDirector.Stop();
            }
            else
            {
                Debug.Log("No PlayableDirector set for CinematicManager");
            }
            EventManager.OnCinematicEnd();
            
            _currentCinematicCoroutine = null;
        }
        
        public void SetCurrentScenePlayableDirector(PlayableDirector playableDirector)
        {
            _currentScenePlayableDirector = playableDirector;
        }
        
        public void ClearCurrentCinematic()
        {
            if (_currentCinematicCoroutine != null)
            {
                StopCoroutine(_currentCinematicCoroutine);
                _currentCinematicCoroutine = null;
            }
        }
    }
}
