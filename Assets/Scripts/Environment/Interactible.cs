using UnityEngine;
using UnityEngine.InputSystem;
using burglar.managers;
using burglar.player;

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

                var interactIcon = other.GetComponent<PlayerToggleInteractionIcon>();
                if (interactIcon != null)
                {
                    interactIcon.interactionIconGO.SetActive(true);
                }

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
            if (_playerInput != null)
            {
                if (_playerInput.actions["Activate"].triggered)
                {
                    try
                    {
                        EventManager.OnInteract(this);
                        Interact();

                        var interactIcon = TutoManager.Instance._player.GetComponent<PlayerToggleInteractionIcon>();
                        if (interactIcon != null)
                        {
                            interactIcon.interactionIconGO.SetActive(false);
                        }

                    } catch (System.NotImplementedException e)
                    {
                        Debug.Log("Interact method not implemented : " + e.Message.ToString());
                    }
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            /*if (_outline != null)
            {
                // Remove the outline
                _outline.enabled = false;
            }*/

            var interactIcon = other.GetComponent<PlayerToggleInteractionIcon>();
            if (interactIcon != null)
            {
                interactIcon.interactionIconGO.SetActive(false);
            }

            _playerInput = null;
        }

        protected virtual void Interact() {}
    }
}
