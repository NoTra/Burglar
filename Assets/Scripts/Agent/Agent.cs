using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using burglar.managers;

namespace burglar.agent
{
    public class Agent : MonoBehaviour
    {
        public enum State
        {
            Patrol,
            Suspicious,
            Search,
            Chase,
            PlayerCaught
        }

        private State _currentState;

        [SerializeField] DecalProjector _decalProjector;
        public float _earingDistance = 5f;

        [SerializeField] private GameObject _suspiciousIcon;

        private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed = 3.5f;
        [SerializeField] private float _chaseSpeed = 7f;

        private Patrol _patrol;
        
        private Vector3 _agentStartPosition;
        private Vector3 _agentStartRotation;
        
        private bool _freezeAgent = false;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _patrol = GetComponent<Patrol>();
            
            _agentStartPosition = transform.position;
            _agentStartRotation = transform.eulerAngles;
        }

        private void Start()
        {
            ChangeState(State.Patrol);

            // Setup the decal projector size based on the earing distance of the agent * 2
            _decalProjector.size = new Vector3(_earingDistance * 2, _earingDistance * 2, 10f);
        }

        private void OnEnable()
        {
            EventManager.ChangeGameState += OnChangeGameState;
            EventManager.DialogStart += OnDialogStart;
            EventManager.DialogEnd += OnDialogEnd;
            EventManager.PlayerCaught += OnPlayerCaught;
        }

        private void OnDisable()
        {
            EventManager.ChangeGameState -= OnChangeGameState;
            EventManager.DialogStart -= OnDialogStart;
            EventManager.DialogEnd -= OnDialogEnd;
            EventManager.PlayerCaught -= OnPlayerCaught;
        }
        
        private void OnPlayerCaught(GameObject arg0)
        {
            ChangeState(State.PlayerCaught);
        }

        public Patrol GetPatrol()
        {
            return _patrol;
        }
        
        private void OnDialogEnd()
        {
            // "unpause" the agent
            _navMeshAgent.isStopped = false;
        }

        private void OnDialogStart()
        {
            // "pause" the agent
            _navMeshAgent.isStopped = true;
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
            if (_currentState == newState)
            {
                return;
            }
            
            _currentState = newState;
            
            Debug.Log("Agent state changed to " + _currentState);

            switch(_currentState)
            {
                case State.Patrol:
                    _freezeAgent = false;
                    _suspiciousIcon.SetActive(false);
                    _navMeshAgent.speed = _speed;
                    _patrol._searchTimeBetweenPoints = 2f;

                    _animator.SetBool(IsWalking, true);
                    _animator.SetBool(IsRunning, false);
                    
                    _patrol.ResumePatrol();
                    break;
                case State.Suspicious:
                    _freezeAgent = false;
                    _suspiciousIcon.SetActive(true);
                    break;
                case State.Search:
                    _freezeAgent = false;
                    _suspiciousIcon.SetActive(false);
                    break;
                case State.Chase:
                    _suspiciousIcon.SetActive(false);
                    // Increase m_Speed of _navMeshAgent
                    _navMeshAgent.speed = _chaseSpeed;
                    _patrol._searchTimeBetweenPoints = 1f;

                    _animator.SetBool(IsRunning, true);
                    _animator.SetBool(IsWalking, false);
                    break;
                case State.PlayerCaught:
                    transform.LookAt(GameManager.Instance.player.transform);
                    _freezeAgent = true;
                    
                    _patrol.ResetSuspiciousPoint();
                    _navMeshAgent.isStopped = true;
                    _suspiciousIcon.SetActive(true);
                    _animator.SetBool(IsRunning, false);
                    _animator.SetBool(IsWalking, false);
                    break;
                default:
                    break;
            }
        }

        public State GetCurrentState()
        {
            return _currentState;
        }

        public void Reset()
        {
            _patrol.ResetSuspiciousPoint();
            
            transform.position = _agentStartPosition;
            transform.eulerAngles = _agentStartRotation;
            _patrol.SetCurrentWaypointIndex(0);
            
            _patrol.ResumePatrol();
        }

        private void LateUpdate()
        {
            // Prevent decalprojector from rotating with the agent on z axis
            _decalProjector.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        public bool IsFrozen()
        {
            return _freezeAgent;
        }
    }
}
