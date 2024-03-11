using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class Interactible : MonoBehaviour
    {
        private Outline _outline;
        private PlayerInput _playerInput;

        private void Start()
        {
            _outline = GetComponent<Outline>();

            if (_outline == null)
            {
                _outline = gameObject.AddComponent<Outline>();
                _outline.enabled = false;

                _outline.OutlineMode = Outline.Mode.OutlineAll;
                _outline.OutlineColor = UIManager.Instance.OutlineColor;
                _outline.OutlineWidth = UIManager.Instance.OutlineWidth;
                
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
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

        protected virtual void Update()
        {
            if (_playerInput != null)
            {
                if (_playerInput.actions["Activate"].triggered)
                {
                    try
                    {
                        Interact();
                    } catch (System.NotImplementedException e)
                    {
                        Debug.Log("Interact method not implemented : " + e.Message.ToString());
                    }
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (_outline != null)
            {
                // Remove the outline
                _outline.enabled = false;
            } else
            {
                Debug.Log("Pas d'outline");
            }

            _playerInput = null;
        }

        protected virtual void Interact() {}
    }
}
