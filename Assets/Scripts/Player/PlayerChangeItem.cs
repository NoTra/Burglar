using burglar.managers;

namespace burglar.player
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

        private static void OnScroll(float scrollValue)
        {
            // var scrollValue = _player._playerInput.actions["ChangeItem"].ReadValue<float>();
            if (scrollValue == 0) return;
            
            var index = GameManager.Instance.items.IndexOf(GameManager.Instance.selectedItem);

            index += (scrollValue > 0) ? 1 : -1;

            // Si l'index est sup�rieur � la taille de la liste, on revient � 0
            if (index >= GameManager.Instance.items.Count)
            {
                index = 0;
            }
            // Si l'index est inf�rieur � 0, on revient � la fin de la liste
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
