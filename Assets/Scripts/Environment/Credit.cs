using UnityEngine;
using burglar.managers;

namespace burglar.environment
{
    public class Credit : Interactible
    {
        [SerializeField] private int _value;

        public int Value => _value;

        protected override void Interact()
        {
            EventManager.OnCreditCollected(_value);
            
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundDialogSlideIn);
            
            Destroy(gameObject);
        }
    }
}
