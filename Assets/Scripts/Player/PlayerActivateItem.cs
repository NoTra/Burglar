using burglar.player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class PlayerActivateItem : PlayerSystem
    {
        private PlayerControls _playerControls;

        private LightSwitch _lightSwitch;

        protected override void Awake()
        {
            base.Awake();

            _playerControls = new PlayerControls();
            _playerControls.Player.ActivateItem.performed += ActivateItem;
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void ActivateItem(InputAction.CallbackContext context)
        {
            switch (GameManager.Instance.GetSelectedItemSlug())
            {
                case "remote":
                    if (GameManager.Instance._lightSwitchSelectedByRemote != null)
                    {
                        GameManager.Instance._lightSwitchSelectedByRemote.ToggleLightSwitch();
                    }
                    else
                    {
                        GameManager.Instance._lightSwitchSelectedByRemote = _lightSwitch;
                    }
                    break;
                case "invisibility":
                    StartCoroutine(Invisibility());
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LightSwitch"))
            {
                _lightSwitch = other.GetComponent<LightSwitch>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LightSwitch"))
            {
                _lightSwitch = null;
            }
        }

        private IEnumerator Invisibility()
        {
            // Player is invisible for 5 seconds
            _player.SetInvisibility(true);
            yield return new WaitForSeconds(5f);
            _player.SetInvisibility(false);
        }
    }
}
