using UnityEngine;
using burglar.managers;

namespace burglar.environment
{
    public class Laser : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || GameManager.Instance.GetSelectedItemSlug() == "mirror") return;
            
            // Trigger alarm
            EventManager.OnChangeGameState(GameManager.GameState.Alert);

            // Generate sound
            EventManager.OnSoundGenerated(transform.position, 10f, false);
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            UpdateLaserDisplay();
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
            UpdateLaserDisplay();
        }

        private void UpdateLaserDisplay()
        {
            // Chjeck if the object is illuminated by a spotlight
            bool isIlluminated = IsObjectIlluminated();

            // Display laser if not illuminated
            _meshRenderer.enabled = !isIlluminated;
        }

        private bool IsObjectIlluminated()
        {
            // Get all Light
            var lights = FindObjectsOfType<Light>();

            foreach (var light in lights)
            {
                if (light.type == LightType.Spot)
                {
                    // Check if the light is on
                    if (light.enabled && light.isActiveAndEnabled && light.gameObject.activeInHierarchy)
                    {
                        // Check if the object is in the light cone 
                        Vector3 direction = transform.position - light.transform.position;
                        var angle = Vector3.Angle(light.transform.forward, direction);
                        var halfAngle = light.spotAngle / 2;

                        if (angle < halfAngle)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
