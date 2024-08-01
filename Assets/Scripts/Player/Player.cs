using System;
using UnityEngine;
using UnityEngine.InputSystem;
using burglar.managers;
using burglar.environment;

namespace burglar.player
{
    public class Player : MonoBehaviour
    {
        public PlayerID ID;

        [HideInInspector] public PlayerInput _playerInput;
        [HideInInspector] public Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;
        public Animator PlayerAnimator;

        public bool _isInvisible = false;
        
        private static readonly int Caught = Animator.StringToHash("Caught");
        private static readonly int Surprised = Animator.StringToHash("Surprised");
        private static readonly int Relieved = Animator.StringToHash("Relieved");
        private static readonly int Interact = Animator.StringToHash("Interact");

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            GameManager.Instance.player = this;
            GameManager.Instance.playerInput = _playerInput;
        }

        private void OnEnable()
        {
            EventManager.IsInvisible += () => OnIsInvisible();
            EventManager.IsVisible += () => OnIsVisible();

            EventManager.PlayerCaught += (player) => OnPlayerCaught(player);
            EventManager.ChangeGameState += (state) => OnChangeGameState(state);
            EventManager.EndOfAlertState += () => OnEndOfAlertState();

            EventManager.Interact += (interactible) => OnInteract(interactible);
            
            EventManager.DialogStart += OnDialogStart;
            EventManager.DialogEnd += OnDialogEnd;
            
            EventManager.CinematicStart += _playerInput.DeactivateInput;
            EventManager.CinematicEnd += _playerInput.ActivateInput;
        }

        private void OnDisable()
        {
            EventManager.IsInvisible -= () => OnIsInvisible();
            EventManager.IsVisible -= () => OnIsVisible();

            EventManager.PlayerCaught -= (player) => OnPlayerCaught(player);
            EventManager.ChangeGameState -= (state) => OnChangeGameState(state);
            EventManager.EndOfAlertState -= () => OnEndOfAlertState();

            EventManager.Interact -= (interactible) => OnInteract(interactible);
            
            EventManager.DialogStart -= OnDialogStart;
            EventManager.DialogEnd -= OnDialogEnd;
            
            EventManager.CinematicStart -= _playerInput.DeactivateInput;
            EventManager.CinematicEnd -= _playerInput.ActivateInput;
        }

        private void OnDialogStart()
        {
            _playerInput.DeactivateInput();
        }
        
        private void OnDialogEnd()
        {
            _playerInput.ActivateInput();
        }

        private void OnInteract(Interactible interactible)
        {
            if (!PlayerAnimator)
            {
                return;
            }
            
            PlayerAnimator?.SetTrigger(Interact);
        }

        private void OnPlayerCaught(GameObject player)
        {
            if (!PlayerAnimator)
            {
                return;
            }
            
            PlayerAnimator?.SetTrigger(Caught);
        }

        private void OnEndOfAlertState()
        {
            if (!PlayerAnimator)
            {
                return;
            }
            
            PlayerAnimator?.SetTrigger(Relieved);
        }

        private void OnChangeGameState(GameManager.GameState state)
        {
            switch (state)
            {
                case GameManager.GameState.GameOver:
                    PlayerAnimator?.SetTrigger(Caught);
                    break;
                case GameManager.GameState.Alert:
                    PlayerAnimator?.SetTrigger(Surprised);
                    break;
                case GameManager.GameState.Playing:
                    PlayerAnimator?.SetTrigger(Relieved);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void OnIsInvisible()
        {
            if (!_meshRenderer) return;
            
            var color = _meshRenderer.material.color;
            _meshRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
        }

        private void OnIsVisible()
        {
            if (!_meshRenderer) return;
            
            var color = _meshRenderer.material.color;
            _meshRenderer.material.color = new Color (color.r, color.g, color.b, 1f);
        }

        public void SetInvisibility(bool state)
        {
            _isInvisible = state;

            if (_isInvisible)
            {
                EventManager.OnIsInvisible();
            }
            else
            {
                EventManager.OnIsVisible();
            }
        }

        public void OnPause()
        {
            GameManager.Instance.TogglePause();
        }

        public void OnActivate() { }
    }
}