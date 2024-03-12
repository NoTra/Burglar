using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class LightSwitch : Interactible
    {
        private MeshRenderer _meshRenderer;
        private Color _materialColor;
        [SerializeField] private AnimationCurve _colorChangeCurve;
        public Light _light;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _materialColor = _meshRenderer.material.color;
        }

        protected override void Interact()
        {
            // if player use Activate button, we toggle the light
            ToggleLightSwitch();

            if (!_light.enabled)
            {
                EventManager.OnLightChange(gameObject);
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            _meshRenderer.material = new Material(_meshRenderer.material) { color = _materialColor };
        }

        public void ToggleLightSwitch()
        {
            _light.enabled = !_light.enabled;
        }

        public void TurnOffLight()
        {
            _light.enabled = false;
        }

        public void TurnOnLight()
        {
            _light.enabled = true;
        }
    }
}
