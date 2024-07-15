using UnityEngine;
using burglar.managers;

namespace burglar.environment
{
    public class LightBehavior : MonoBehaviour
    {
        private Animator _blinkAnimator;
        private static readonly int Alert = Animator.StringToHash("Alert");

        private void Start()
        {
            _blinkAnimator = GetComponent<Animator>();
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
                    // Switch on light and start blinking
                    _blinkAnimator?.SetBool(Alert, true);
                    break;
                case GameManager.GameState.Playing:
                        _blinkAnimator?.SetBool(Alert, false);
                    break;
                default:
                    break;
            }
        }
    }
}
