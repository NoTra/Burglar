using UnityEngine;

namespace burglar.managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
