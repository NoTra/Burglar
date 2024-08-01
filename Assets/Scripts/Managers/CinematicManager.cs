using System.Collections;
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

        private void LaunchCinematic()
        {
            var currentLevel = LevelManager.Instance._currentLevel;
            
            if (currentLevel.startCinematic)
            {
                Debug.Log("LaunchCinematic (CinematicManager)");
                StartCoroutine(PlayCinematic(currentLevel.startCinematic));
            }
        }
        
        private IEnumerator PlayCinematic(TimelineAsset cinematic)
        {
            EventManager.OnCinematicStart();
            
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
