using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace burglar
{
    public class Laser : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && GameManager.Instance.GetSelectedItemSlug() != "mirror")
            {
                // Alarme d�clench�e
                EventManager.OnChangeGameState(GameManager.GameState.Alert);

                // On g�n�re un son
                EventManager.OnSoundGenerated(transform.position, 10f, false);
            }
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
            EventManager.LightChange += (switchGO) => OnLightChange(switchGO);
        }

        private void OnDisable()
        {
            EventManager.LightChange -= (switchGO) => OnLightChange(switchGO);
        }

        private void OnLightChange(GameObject switchGO)
        {
            UpdateLaserDisplay();
        }

        private void UpdateLaserDisplay()
        {
            // V�rifie si l'objet est �clair� par une lumi�re de type SpotLight
            bool isLit = IsObjectLitBySpotlight();

            // Display laser if not lit
            _meshRenderer.enabled = !isLit;
        }

        private bool IsObjectLitBySpotlight()
        {
            // R�cup�re toutes les lumi�res de type SpotLight
            Light[] lights = FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                if (light.type == LightType.Spot)
                {
                    // V�rifie si la lumi�re �claire l'objet
                    if (light.enabled && light.isActiveAndEnabled && light.gameObject.activeInHierarchy)
                    {
                        // V�rifie si l'objet est dans le c�ne de la lumi�re
                        Vector3 direction = transform.position - light.transform.position;
                        float angle = Vector3.Angle(light.transform.forward, direction);
                        float halfAngle = light.spotAngle / 2;

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
