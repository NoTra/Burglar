using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace burglar
{
    public class SceneManager : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            // Load the scene in Single mode
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
