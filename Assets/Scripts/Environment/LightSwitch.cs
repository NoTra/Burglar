using UnityEngine;
using System.Collections.Generic;
using burglar.managers;

namespace burglar.environment
{
    public class LightSwitch : Interactible
    {
        private MeshRenderer _meshRenderer;
        private Color _materialColor;
        [SerializeField] private AnimationCurve colorChangeCurve;
        public List<Light> _lights;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _materialColor = _meshRenderer.material.color;
        }

        protected override void Interact()
        {
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundLightSwitch);
            
            ToggleLightSwitch();
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            _meshRenderer.material = new Material(_meshRenderer.material) { color = _materialColor };
        }

        public void ToggleLightSwitch()
        {
            foreach (var light in _lights)
            {
                light.enabled = !light.enabled;
            }

            // Trigger light change event
            EventManager.OnLightChange(gameObject);
        }

        public void TurnOffLight()
        {
            foreach (var light in _lights)
            {                 
                light.enabled = false;
            }
        }

        public void TurnOnLight()
        {
            foreach (var light in _lights)
            {
                light.enabled = true;
            }
        }
    }
}
