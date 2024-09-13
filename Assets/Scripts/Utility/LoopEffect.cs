using System;
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
        private void Awake()
        {
            _visualEffects = new List<VisualEffect>(GetComponentsInChildren<VisualEffect>());
        }
        
        private IEnumerator LoopEffectCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delay);
                foreach (var effect in _visualEffects)
                {
                    effect.SendEvent("Click");
                }
            }
        }

        private void OnEnable()
        {
            StartCoroutine(LoopEffectCoroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
