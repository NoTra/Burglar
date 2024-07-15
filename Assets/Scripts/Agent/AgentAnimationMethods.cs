using UnityEngine;
using burglar.managers;

namespace burglar
{
    public class AgentAnimationMethods : MonoBehaviour
    {
        [SerializeField] private AudioSource _agentAudioSource;
        
        public void OnFootStep()
        {
            var audioManager = AudioManager.Instance;
            _agentAudioSource.PlayOneShot(audioManager.soundFootstep);
        }
    }
}
