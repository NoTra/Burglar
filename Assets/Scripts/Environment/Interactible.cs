using UnityEngine;
using UnityEngine.InputSystem;
using burglar.managers;

namespace burglar.environment
{
    public class Interactible : MonoBehaviour
    {
        private Outline _outline;
        private PlayerInput _playerInput;

        private void Start()
        {
            _outline = GetComponent<Outline>();

            if (_outline != null)
            {
                _outline.enabled = false;

                _outline.OutlineMode = Outline.Mode.OutlineVisible;
                _outline.OutlineColor = UIManager.Instance ? UIManager.Instance.OutlineColor : Color.white;
                _outline.OutlineWidth = UIManager.Instance ? UIManager.Instance.OutlineWidth : 2f;
                
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered the trigger of: " + gameObject.name);

                EventManager.OnEnterInteractibleArea(this);

                if (_outline != null)
                {
                    // Outline the object
                    _outline.enabled = true;
                }

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
            Debug.Log("Interact");
        }
    }
}
