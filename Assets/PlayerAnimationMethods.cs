using System.Collections;
using System.Collections.Generic;
using burglar.managers;
using UnityEngine;

namespace burglar
{
    public class PlayerAnimationMethods : MonoBehaviour
    {
        public void OnFootStep()
        {
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundFootstep);
        }
        
        public void OnSneakyFootStep()
        {
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundSneakyFootstep);
        }
    }
}
