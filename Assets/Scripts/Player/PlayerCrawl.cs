using burglar.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class PlayerCrawl : PlayerSystem
    {
        private Rigidbody _playerRB;
        private CapsuleCollider _playerCollider;
        private float _capsuleColliderHeight;
        [SerializeField] private Animator _playerAnimator;

        protected override void Awake()
        {
            base.Awake();
            _playerRB = GetComponent<Rigidbody>();
            _playerCollider = GetComponent<CapsuleCollider>();

            _capsuleColliderHeight = _playerCollider.height;
        }

        void Update()
        {
            /*var YRotation = _playerRB.rotation.eulerAngles.y;

            if (_player._playerInput.actions["Crawl"].ReadValue<float>() > 0)
            {
                // Rotation X à 90°
                _playerRB.rotation = Quaternion.Euler(90, YRotation, 0);
            } else { 
                // Rotation X à 0°
                _playerRB.rotation = Quaternion.Euler(0, YRotation, 0);
            }*/

            // Reduce player collider height when crawling
            if (_player._playerInput.actions["Crawl"].ReadValue<float>() > 0)
            {
                if (_playerCollider.height == _capsuleColliderHeight)
                {
                    Debug.Log("Crouch");
                    _playerAnimator.SetBool("Crouch", true);
                }
                _playerCollider.height = 0.5f;
            }
            else
            {
                _playerAnimator.SetBool("Crouch", false);
                _playerCollider.height = _capsuleColliderHeight;
            }
        }
    }
}
