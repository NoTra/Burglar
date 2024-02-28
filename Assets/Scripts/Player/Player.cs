using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar.player
{
    public class Player : MonoBehaviour
    {
        public PlayerID ID;

        [HideInInspector] public PlayerInput _playerInput;
        [HideInInspector] public Rigidbody _rigidbody;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}