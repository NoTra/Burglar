using UnityEngine;

namespace burglar
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
    public class LevelSO : ScriptableObject
    {
        public string sceneName;
        public string levelName;
        public int levelIndex;
        public int minimumCredits;
        public int maximumCredits;
    }
}
