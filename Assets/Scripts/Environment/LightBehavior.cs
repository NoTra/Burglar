using UnityEngine;

namespace burglar
{
    public class LightBehavior : MonoBehaviour
    {
        private Animator _blinkAnimator;

        private void Start()
        {
            _blinkAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            EventManager.ChangeGameState += (state) => OnChangeGameState(state);
        }

        private void OnDisable()
        {
            EventManager.ChangeGameState -= (state) => OnChangeGameState(state);
        }

        private void OnChangeGameState(GameManager.GameState state)
        {
            switch (state)
            {
                case GameManager.GameState.Alert:
                    if (_blinkAnimator != null)
                    {
                        _blinkAnimator.SetBool("Alert", true);
                    }
                    break;
                case GameManager.GameState.Playing:
                    if (_blinkAnimator != null)
                    {
                        _blinkAnimator.SetBool("Alert", false);
                    }
                    break;
                default:
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
