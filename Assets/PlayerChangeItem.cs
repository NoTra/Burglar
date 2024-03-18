using burglar.player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class PlayerChangeItem : PlayerSystem
    {
        private PlayerControls _playerControls;

        protected override void Awake()
        {
            base.Awake();

            _playerControls = new PlayerControls();
            _playerControls.Player.ChangeItem.performed += ctx => OnScroll(ctx.ReadValue<float>());
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void OnScroll(float scrollValue)
        {
            // var scrollValue = _player._playerInput.actions["ChangeItem"].ReadValue<float>();
            Debug.Log("Scroll value: " + scrollValue);
            if (scrollValue != 0)
            {
                var index = GameManager.Instance.items.IndexOf(GameManager.Instance.selectedItem);

                if (scrollValue > 0)
                {
                    Debug.Log("ChangeItem --->");
                    index++;

                }
                else
                {
                    Debug.Log("ChangeItem <---");
                    index--;
                }

                // Si l'index est supérieur à la taille de la liste, on revient à 0
                if (index >= GameManager.Instance.items.Count)
                {
                    index = 0;
                }
                // Si l'index est inférieur à 0, on revient à la fin de la liste
                else if (index < 0)
                {
                    index = GameManager.Instance.items.Count - 1;
                }

                // Change selected item
                GameManager.Instance.selectedItem = GameManager.Instance.items[index];

                // Trigger event OnSelectItem
                EventManager.OnSelectItem(GameManager.Instance.selectedItem);
            }
        }
    }
}
