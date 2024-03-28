using System;
using UnityEngine;

namespace burglar.player
{
    public class PlayerMovement : PlayerSystem
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _runSpeed = 6f;
        [SerializeField] private float _crawlSpeed = 1.5f;

        private float _lastSoundTime = 0f;
        private float _soundTriggerDelay = 0.2f;
        [SerializeField] private float _stepStrength = 0.2f;
        private bool _wasRunning = false;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            // Si le joueur appuie sur la touche "Run" (holding shift)
            UpdateSpeed();

            // _meshRenderer.material.color = (_speed == _runSpeed) ? new Color(255f, 0, 0, _meshRenderer.material.color.a) : _startColor;

            // On récupère l'input du joueur
            // Vector2 moveDirection = _player._playerInput.actions["Movement"].ReadValue<Vector2>();

            // On passe à l'animator que la variable velocity = Player.GetRigidbody().velocity.magnitude
            // _player.GetAnimator().SetBool("isRunning", (moveDirection != Vector2.zero));
        }

        private void UpdateSpeed()
        {
            if (_player._playerInput.actions["Crawl"].ReadValue<float>() > 0)
            {
                _speed = _crawlSpeed;
            }
            else if (_player._playerInput.actions["Run"].ReadValue<float>() > 0)
            {
                _speed = _runSpeed;
            }
            else
            {
                _speed = _walkSpeed;
            }
        }

        private void FixedUpdate()
        {
            Vector2 moveDirection = _player._playerInput.actions["Movement"].ReadValue<Vector2>();

            // Rotate player to the direction of the movement
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                var xRotation = _player._rigidbody.transform.rotation.eulerAngles.x;
                _player._rigidbody.transform.rotation = Quaternion.Euler(xRotation, angle, 0f);
            }

            var position = _player._rigidbody.transform.position;

            // Moving player
            _player._rigidbody.transform.position = position + _speed * Time.fixedDeltaTime * new Vector3(moveDirection.x, 0, moveDirection.y);

            if (moveDirection != Vector2.zero)
            {
                if (_speed == _walkSpeed)
                {
                    _player.PlayerAnimator.SetBool("isWalking", true);
                    _player.PlayerAnimator.SetBool("isRunning", false);
                    _player.PlayerAnimator.SetBool("isCrawling", false);

                    _player.PlayerAnimator.SetBool("Crouch", false);
                }
                else if (_speed == _runSpeed)
                {
                    _player.PlayerAnimator.SetBool("isRunning", true);
                    _player.PlayerAnimator.SetBool("isWalking", false);
                    _player.PlayerAnimator.SetBool("isCrawling", false);

                    _player.PlayerAnimator.SetBool("Crouch", false);
                }
                else
                {
                    Debug.Log("Crawling");
                    _player.PlayerAnimator.SetBool("isCrawling", true);
                    _player.PlayerAnimator.SetBool("isWalking", false);
                    _player.PlayerAnimator.SetBool("isRunning", false);

                    _player.PlayerAnimator.SetBool("Crouch", false);
                }

                // If the player is running, we generate a sound
                if (_speed > _walkSpeed && (Time.time - _lastSoundTime > _soundTriggerDelay))
                {
                    EventManager.OnSoundGenerated(position, _stepStrength, true);
                    _lastSoundTime = Time.time;
                    _wasRunning = true;
                }
                else
                {
                    // Sound at the end of the run (if > 1s
                    if (_wasRunning)
                    {
                        if (Time.time - _lastSoundTime > 1f)
                        {
                            EventManager.OnSoundGenerated(position, _stepStrength, true);
                        }

                        // We generate a sound when the player stops running
                        _wasRunning = false;
                    }
                }
            } else
            {
                _player.PlayerAnimator.SetBool("isRunning", false);
                _player.PlayerAnimator.SetBool("isWalking", false);
                _player.PlayerAnimator.SetBool("isCrawling", false);
            }
        }
    }
}