using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using burglar.levels;

namespace burglar.managers
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
    public class LevelSO : ScriptableObject
    {
        public string sceneName;
        public string levelName;
        public int levelIndex;
        public int minimumCredits;
        public AudioClip music;
        public bool resetCredits = false;
        public TimelineAsset startCinematic;
    }
}
