using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace burglar
{
    public class AlertLevel : MonoBehaviour
    {
        private float _alertLevel = 0.0f;
        private float _alertLevelDecrease = 0.1f;
        private float _maxAlertLevel = 1.0f;
        private float _waitTimeBeforeDecrease = 1.0f;
        private float _elapsedTime = 0;

        private Agent _agent;

        [SerializeField] private Slider _alertSlider;

        private void Awake()
        {
            _agent = GetComponent<Agent>();
        }

        private void OnEnable()
        {
            EventManager.SoundHeard += (point, strength) => IncreaseAlertLevel(point, strength);
        }

        private void OnDisable()
        {
            EventManager.SoundHeard -= (point, strength) => IncreaseAlertLevel(point, strength);
        }

        private void IncreaseAlertLevel(Vector3 point, float strength)
        {
            if (Vector3.Distance(transform.position, point) > _agent._earingDistance)
            {
                Debug.Log("Sound is too far, no alert");
                return;
            }

            _alertLevel = Mathf.Clamp01(_alertLevel + strength);
        }

        private void UpdateAlertLevel()
        {
            _alertSlider.value = _alertLevel;
            if (_alertLevel >= _maxAlertLevel)
            {
                Debug.Log("Alarm triggered !");
                // On déclenche l'alarme
                // EventManager.AlarmTriggered?.Invoke();
            }
        }

        private void Update()
        {
            if (_alertLevel > 0 )
            {
                if (_waitTimeBeforeDecrease < _elapsedTime)
                {
                    _elapsedTime += Time.deltaTime;
                } else
                {
                    DicreaseAlertLevel(_alertLevelDecrease);

                    _elapsedTime = 0;
                }
            }
        }

        private void DicreaseAlertLevel(float dicreaseValue)
        {
            _alertLevel = Mathf.Clamp01(_alertLevel - (dicreaseValue * Time.deltaTime));
            UpdateAlertLevel();
        }
    }
}
