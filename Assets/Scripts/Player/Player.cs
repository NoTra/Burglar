using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar.player
{
    public class Player : MonoBehaviour
    {
        public PlayerID ID;

        [HideInInspector] public PlayerInput _playerInput;
        [HideInInspector] public Rigidbody _rigidbody;
        MeshRenderer _meshRenderer;

        public bool _isInvisible = false;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            EventManager.IsInvisible += () => OnIsInvisible();
            EventManager.IsVisible += () => OnIsVisible();
        }

        private void OnDisable()
        {
            EventManager.IsInvisible -= () => OnIsInvisible();
            EventManager.IsVisible -= () => OnIsVisible();
        }

        private void OnIsInvisible()
        {
            Debug.Log("OnIsInvisible called");
            var color = _meshRenderer.material.color;
            _meshRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
        }

        private void OnIsVisible()
        {
            Debug.Log("OnIsVisible called");
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
    }
}