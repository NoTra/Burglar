using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace burglar
{
    public class LoopEffect : MonoBehaviour
    {
        [SerializeField] private float _delay = 1f;
        private List<VisualEffect> _visualEffects;
        
        // Start is called before the first frame update
        private void Start()
        {
            _visualEffects = new List<VisualEffect>(GetComponentsInChildren<VisualEffect>());
            
            StartCoroutine(LoopEffectCoroutine());
        }
        
        private IEnumerator LoopEffectCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delay);
                foreach (var effect in _visualEffects)
                {
                    Debug.Log("Effect for " + effect.gameObject.name + " triggered.");
                    effect.SendEvent("Click");
                }
            }
        }
        
    }
}
