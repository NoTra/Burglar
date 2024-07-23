using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace burglar
{
    public class SceneLoader : MonoBehaviour
    {
        private void Awake()
        {
            // Si le GameManager n'existe pas dans la scène, on load la scène "bootstrap" et on attend qu'il soit chargé avant de continuer de charger la scène "main"
            if (managers.GameManager.Instance == null)
            {
                // On attend que la scène "bootstrap" soit chargée
                StartCoroutine(LoadGameManagerAndWaitForIt());
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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
