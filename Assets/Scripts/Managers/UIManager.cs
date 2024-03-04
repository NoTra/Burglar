using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject UIGameOverPanel;

        private void OnEnable()
        {
            EventManager.PlayerCaught += (player) => OnPlayerCaught(player);
        }

        private void OnDisable()
        {
            EventManager.PlayerCaught -= (player) => OnPlayerCaught(player);
        }

        private void OnPlayerCaught(GameObject player)
        {
            UIGameOverPanel.SetActive(true);

            StartCoroutine(WaitBeforeRestart());
        }

        private IEnumerator WaitBeforeRestart()
        {
            yield return new WaitForSeconds(3.0f);

            // Stop All Coroutines
            StopAllCoroutines();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
