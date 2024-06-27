using UnityEngine;

namespace burglar.player
{
    public class PlayerCrawl : PlayerSystem
    {
        private CapsuleCollider _playerCollider;
        private float _capsuleColliderHeight;
        [SerializeField] private Animator _playerAnimator;

        protected override void Awake()
        {
            base.Awake();
            _playerCollider = GetComponent<CapsuleCollider>();

            _capsuleColliderHeight = _playerCollider.height;
        }

        void Update()
        {
            // Reduce player collider height when crawling
            if (_player._playerInput.actions["Crawl"].ReadValue<float>() > 0)
            {
                if (_playerCollider.height == _capsuleColliderHeight)
                {
                    _playerAnimator.SetBool("Crouch", true);
                }
                _playerCollider.height = 0.5f;
            }
            else
            {
                if (_playerAnimator.GetBool("Crouch"))
                {
                    _playerAnimator.SetBool("Crouch", false);
                }

                _playerCollider.height = _capsuleColliderHeight;
            }
        }
    }
}
