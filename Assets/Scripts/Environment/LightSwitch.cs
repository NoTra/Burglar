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
            ToggleLightSwitch();
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            _meshRenderer.material = new Material(_meshRenderer.material) { color = _materialColor };
        }

        public void ToggleLightSwitch()
        {
            _light.enabled = !_light.enabled;

            // Trigger light change event
            EventManager.OnLightChange(gameObject);
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
