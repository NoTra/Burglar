using UnityEngine;
using UnityEngine.InputSystem;
using burglar.managers;

namespace burglar.environment
{
    public class Interactible : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private void Start()
        {
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.OnEnterInteractibleArea(this);

                if (_playerInput == null)
                {
                    _playerInput = other.GetComponent<PlayerInput>();
                }
            }
        }

        protected virtual void Update()
        {
            if (!_playerInput || !_playerInput.actions["Activate"].triggered) return;

            try
            {
                EventManager.OnInteract(this);
                Interact();

            } catch (System.NotImplementedException e)
            {
                Debug.Log("Interact method not implemented : " + e.Message);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            EventManager.OnExitInteractibleArea(this);

            _playerInput = null;
        }

        protected virtual void Interact()
        {
        }
    }
}
