using UnityEngine;

namespace burglar.player
{
    public class PlayerMovement : PlayerSystem
    {
        [SerializeField] private float _speed = 2f;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            // On récupère l'input du joueur
            // Vector2 moveDirection = _player._playerInput.actions["Movement"].ReadValue<Vector2>();

            // On passe à l'animator que la variable velocity = Player.GetRigidbody().velocity.magnitude
            // _player.GetAnimator().SetBool("isRunning", (moveDirection != Vector2.zero));
        }

        private void FixedUpdate()
        {
            // On récupère l'input du joueur
            Vector2 moveDirection = _player._playerInput.actions["Movement"].ReadValue<Vector2>();

            // Rotation du joueur en fonction de la direction du movement
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                _player._rigidbody.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            var position = _player._rigidbody.transform.position;
            // On déplace le joueur
            _player._rigidbody.transform.position = position + new Vector3(moveDirection.x, 0, moveDirection.y) * _speed * Time.fixedDeltaTime;
        }
    }
}