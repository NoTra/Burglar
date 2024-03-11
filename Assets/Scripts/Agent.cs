using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class Agent : MonoBehaviour
    {
        public enum State
        {
            Patrol,
            Suspicious,
            Search,
            Chase
        }

        private State _currentState;

        public float _earingDistance = 10f;

        [SerializeField] private GameObject _suspiciousIcon;

        private void Start()
        {
            ChangeState(State.Patrol);
        }

        public void ChangeState(State newState)
        {
            _currentState = newState;

            switch(_currentState)
            {
                case State.Patrol:
                    _suspiciousIcon.SetActive(false);
                    break;
                case State.Suspicious:
                    _suspiciousIcon.SetActive(true);
                    break;
                case State.Search:
                    _suspiciousIcon.SetActive(false);
                    break;
                case State.Chase:
                    _suspiciousIcon.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public State GetCurrentState()
        {
            return _currentState;
        }
    }
}
