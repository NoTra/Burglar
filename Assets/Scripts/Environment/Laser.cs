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
                // Alarme déclenchée
                EventManager.OnChangeGameState(GameManager.GameState.Alert);

                // On génère un son
                // EventManager.OnSoundGenerated(transform.position, 10f, false);
            }
        }

        private void OnDestroy()
        {
            Debug.Log("[LASER] OnDestroy (" + gameObject.name + ")");
        }

        private void Awake()
        {
            Debug.Log("[LASER] Awake (" + gameObject.name + ")");
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            Debug.Log("[LASER] Start (" + gameObject.name + ")");
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
            Debug.Log("Laser.cs OnLightChange...");
            UpdateLaserDisplay();
        }

        private void UpdateLaserDisplay()
        {
            // Vérifie si l'objet est éclairé par une lumière de type SpotLight
            bool isLit = IsObjectLitBySpotlight();

            if (isLit)
            {
                _meshRenderer.enabled = false;
                Debug.Log("Laser est éclairé");
            }
            else
            {
                _meshRenderer.enabled = true;
                Debug.Log("Laser est dans la pénombre");
            }
        }

        private bool IsObjectLitBySpotlight()
        {
            // Récupère toutes les lumières de type SpotLight
            Light[] lights = FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                if (light.type == LightType.Spot)
                {
                    // Vérifie si la lumière éclaire l'objet
                    if (light.enabled && light.isActiveAndEnabled && light.gameObject.activeInHierarchy)
                    {
                        // Vérifie si l'objet est dans le cône de la lumière
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
