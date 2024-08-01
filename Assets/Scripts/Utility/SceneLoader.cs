using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using burglar.managers;

namespace burglar.utility
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector; 
        
        private void Awake()
        {
            // Si le GameManager n'existe pas dans la scène, on load la scène "bootstrap" et on attend qu'il soit chargé avant de continuer de charger la scène "main"
            if (managers.GameManager.Instance == null)
            {
                // On attend que la scène "bootstrap" soit chargée
                StartCoroutine(LoadGameManagerAndWaitForIt());
            }
        }

        private void OnEnable()
        {
            if (playableDirector)
            {
                Debug.Log("SetCurrentScenePlayableDirector to CinematicManager");
                CinematicManager.Instance.SetCurrentScenePlayableDirector(playableDirector);
            }
        }

        private IEnumerator LoadGameManagerAndWaitForIt()
        {
            Debug.Log("GameManager not found, loading bootstrap scene");
            if (!managers.GameManager.Instance)
            {
                // On charge la scène "main"
                UnityEngine.SceneManagement.SceneManager.LoadScene("bootstrap", LoadSceneMode.Additive);
                
                yield return new WaitUntil(() => managers.GameManager.Instance);
            }
        }
    }
}
