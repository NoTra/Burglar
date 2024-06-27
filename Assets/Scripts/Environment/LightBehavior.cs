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
                    if (_blinkAnimator != null)
                    {
                        _blinkAnimator.SetBool(Alert, true);
                    }
                    break;
                case GameManager.GameState.Playing:
                    if (_blinkAnimator != null)
                    {
                        _blinkAnimator.SetBool(Alert, false);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
