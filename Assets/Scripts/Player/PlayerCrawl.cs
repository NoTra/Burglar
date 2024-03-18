using burglar.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class PlayerCrawl : PlayerSystem
    {
        private Rigidbody _playerRB;

        protected override void Awake()
        {
            base.Awake();
            _playerRB = GetComponent<Rigidbody>();
        }

        void Update()
        {
            var YRotation = _playerRB.rotation.eulerAngles.y;

            if (_player._playerInput.actions["Crawl"].ReadValue<float>() > 0)
            {
                // Rotation X à 90°
                _playerRB.rotation = Quaternion.Euler(90, YRotation, 0);
            } else { 
                // Rotation X à 0°
                _playerRB.rotation = Quaternion.Euler(0, YRotation, 0);
            }
        }
    }
}
