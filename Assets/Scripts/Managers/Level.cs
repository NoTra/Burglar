using System.Collections.Generic;
using UnitySceneManagement = UnityEngine.SceneManagement;
using MonoBehavior = UnityEngine.MonoBehaviour;

namespace burglar.managers
{
    public class Level : MonoBehavior
    {
        public UnitySceneManagement.Scene Scene;
        private string _sceneName;
        private int _buildId;
        public bool isCompleted;
        public bool isCurrent;

        private void Awake()
        {
            _sceneName = Scene.name;
            _buildId = Scene.buildIndex;
        }
    }
}