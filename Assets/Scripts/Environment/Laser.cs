using UnityEngine;
using System.Collections.Generic;
using burglar.managers;
using burglar.player;

namespace burglar.environment
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private List<Light> _lights;

        private void OnTriggerEnter(Collider other)
        {
            if (
                !other.CompareTag("Player") || // Only player can trigger alarm
                GameManager.Instance.GetSelectedItemSlug() == "mirror" || // Mirror can't trigger alarm
                other.GetComponent<Player>().PlayerAnimator.GetBool("isCrawling")
            ) return;
            
            // Trigger alarm
            EventManager.OnChangeGameState(GameManager.GameState.Alert);

            // Generate sound
            EventManager.OnSoundGenerated(transform.position, 10f, false);
        }

        private void Start()
        {
            var isIlluminated = false;
            foreach (var light in _lights)
            {
                if (light.isActiveAndEnabled)
                {
                    isIlluminated = true;
                    break;
                }
            }
            
            _meshRenderer.enabled = !isIlluminated;
        }

        private void OnEnable()
        {
            EventManager.LightChange += OnLightChange;
        }

        private void OnDisable()
        {
            EventManager.LightChange -= OnLightChange;
        }

        private void OnLightChange(GameObject switchGO)
        {
            // On récupère les lights contrôlées par le switch
            var lights = switchGO.GetComponent<LightSwitch>()._lights;
            
            // On check si une de ces lights peut affecter le laser
            foreach(var light in lights)
            {
                if (_lights.Contains(light))
                {
                    UpdateLaserDisplay(lights);
                    break;
                }
            }
        }

        private void UpdateLaserDisplay(List<Light> lightsCommandedBySwitch)
        {
            var isIlluminated = false;
            foreach (var lightOfSwitch in lightsCommandedBySwitch)
            {
                // if (!light.isActiveAndEnabled) continue;
                
                if (_lights.Contains(lightOfSwitch))
                {
                    isIlluminated = lightOfSwitch.isActiveAndEnabled;
                    break;
                }
            }
            
            // Display laser if not illuminated
            _meshRenderer.enabled = !isIlluminated;
        }
    }
}
