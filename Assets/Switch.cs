using burglar.player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class Switch : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private Color _materialColor;
        [SerializeField] private AnimationCurve _colorChangeCurve;
        private Coroutine _blinkCoroutine;
        [SerializeField] Light _light;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _materialColor = _meshRenderer.material.color;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_blinkCoroutine == null)
            {
                _blinkCoroutine = StartCoroutine(BlinkColor());
            }

            if (_playerInput == null)
            {
                _playerInput = other.GetComponent<PlayerInput>();
            }

            // if player use Activate button, we toggle the light
            if (_playerInput != null && _playerInput.actions["Activate"].triggered)
            {
                Debug.Log("E pressed!");
                _light.enabled = !_light.enabled;
            }
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

        private void OnTriggerExit(Collider other)
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _blinkCoroutine = null;
            }

            _meshRenderer.material = new Material(_meshRenderer.material) { color = _materialColor };
        }
    }
}
