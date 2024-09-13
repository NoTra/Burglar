using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using burglar.managers;
using EasyTransition;

namespace burglar.utility
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector; 
        
        private void Awake()
        {
            // Si le GameManager n'existe pas dans la scène, on load la scène "bootstrap" et on attend qu'il soit chargé avant de continuer de charger la scène "main"
            if (GameManager.Instance == null)
            {
                // On attend que la scène "bootstrap" soit chargée
                StartCoroutine(LoadGameManagerAndWaitForIt());
            }
        }

        private void Start()
        {
            StartCoroutine(WaitForSecondsAndEndTransition(1f));
            
            if (!playableDirector) return;
            
            CinematicManager.Instance.SetCurrentScenePlayableDirector(playableDirector);
            
            // On lance la cinématique
            CinematicManager.Instance.LaunchCinematic(playableDirector.playableAsset);
        }
        
        private IEnumerator WaitForSecondsAndEndTransition(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            TransitionManager.Instance().onTransitionEnd?.Invoke();
            TransitionManager.Instance().SetRunningTransition(false);
        }

        private IEnumerator LoadGameManagerAndWaitForIt()
        {
            if (!GameManager.Instance)
            {
                // On charge la scène "main"
                SceneManager.LoadScene("bootstrap", LoadSceneMode.Additive);
                
                yield return new WaitUntil(() => GameManager.Instance);
            }
        }
    }
}
