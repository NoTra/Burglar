using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

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

        [SerializeField] DecalProjector _decalProjector;
        public float _earingDistance = 5f;

        [SerializeField] private GameObject _suspiciousIcon;

        private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _speed = 3.5f;
        [SerializeField] private float _chaseSpeed = 7f;

        private Patrol _patrol;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            ChangeState(State.Patrol);

            EventManager.ChangeGameState += (state) => OnChangeGameState(state);

            // Setup the decal projector size based on the earing distance of the agent * 2
            _decalProjector.size = new Vector3(_earingDistance * 2, _earingDistance * 2, 10f);

            Debug.Log("Earing size changed to : " + _decalProjector.size);
        }

        private void OnChangeGameState(GameManager.GameState state)
        {
            switch (state)
            {
                case GameManager.GameState.Alert:
                    ChangeState(State.Chase);
                    break;
                case GameManager.GameState.Playing:
                    ChangeState(State.Patrol);
                    break;
                default:
                    break;
            }
        }

        public void ChangeState(State newState)
        {
            _currentState = newState;

            switch(_currentState)
            {
                case State.Patrol:
                    _suspiciousIcon.SetActive(false);
                    _navMeshAgent.speed = _speed;
                    _patrol._searchTimeBetweenPoints = 2f;
                    break;
                case State.Suspicious:
                    _suspiciousIcon.SetActive(true);
                    break;
                case State.Search:
                    _suspiciousIcon.SetActive(false);
                    break;
                case State.Chase:
                    _suspiciousIcon.SetActive(false);
                    // Increase m_Speed of _navMeshAgent
                    _navMeshAgent.speed = _chaseSpeed;
                    _patrol._searchTimeBetweenPoints = 1f;
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
