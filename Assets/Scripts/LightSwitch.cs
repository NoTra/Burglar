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
        private Coroutine _blinkCoroutine;
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

            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _blinkCoroutine = null;
            }

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

        private IEnumerator BlinkColor()
        {
            // From current color to green
            float elapsedTime = 0;
            float cycleDuration = 1f;

            while (elapsedTime < cycleDuration)
            {
                elapsedTime += Time.deltaTime;
                Color color = Color.Lerp(_materialColor, Color.green, _colorChangeCurve.Evaluate(elapsedTime / cycleDuration));
                _meshRenderer.material = new Material(_meshRenderer.material) { color = color };
                yield return null;
            }
            _meshRenderer.material = new Material(_meshRenderer.material) { color = Color.green };

            // From green to current color
            elapsedTime = 0;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                Color color = Color.Lerp(Color.green, _materialColor, _colorChangeCurve.Evaluate(elapsedTime / cycleDuration));
                _meshRenderer.material = new Material(_meshRenderer.material) { color = color };
                yield return null;
            }
            _meshRenderer.material = new Material(_meshRenderer.material) { color = _materialColor };
            yield return null;
            _blinkCoroutine = null;
        }
    }
}
