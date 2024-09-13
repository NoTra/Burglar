using System;
using System.Collections;
using UnityEngine;
using burglar.managers;

namespace burglar.environment
{
    public class LightBehavior : MonoBehaviour
    {
        private Animator _blinkAnimator;
        private static readonly int Alert = Animator.StringToHash("Alert");

        private bool firstLightState;
        private bool previousLightState;
        
        private Light _light;

        private void Awake()
        {
            _blinkAnimator = GetComponent<Animator>();
            _light = GetComponent<Light>();
            
            firstLightState = _light.enabled;
        }

        private void OnEnable()
        {
            EventManager.ChangeGameState += OnChangeGameState;
        }

        private void OnDisable()
        {
            EventManager.ChangeGameState -= OnChangeGameState;
        }

        private void OnChangeGameState(GameManager.GameState state)
        {
            switch (state)
            {
                case GameManager.GameState.Alert:
                    // Store previous light state
                    previousLightState = _light.enabled;
                    
                    // Switch on light and start blinking
                    _light.enabled = true;
                    _blinkAnimator?.SetBool(Alert, true);
                break;
                case GameManager.GameState.Playing:
                    // Stop blinking
                    _blinkAnimator?.SetBool(Alert, false);
                    
                    // Restore previous light state
                    _light.enabled = previousLightState;
                break;
                default:
                    break;
            }
        }

        private IEnumerator ResetPreviousLightStateAfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _light.enabled = previousLightState;
        }
        
        public void ResetLightState()
        {
            if (_light.enabled == firstLightState) return;
            
            Debug.Log("Resetting light state for " + gameObject.name);
            _light.enabled = firstLightState;
        }
    }
}
