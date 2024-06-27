using UnityEngine;
using UnityEngine.UI;
using burglar.managers;

namespace burglar.agent
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
            EventManager.SoundGenerated += (point, strength, checkDistance) => IncreaseAlertLevel(point, strength, checkDistance);
            EventManager.EndOfAlertState += () => ResetAlertLevel();
        }

        private void OnDisable()
        {
            EventManager.SoundGenerated -= (point, strength, checkDistance) => IncreaseAlertLevel(point, strength, checkDistance);
            EventManager.EndOfAlertState -= () => ResetAlertLevel();
        }

        private void ResetAlertLevel()
        {
            _alertLevel = 0;
            UpdateAlertLevel();
        }

        private void IncreaseAlertLevel(Vector3 point, float strength, bool checkDistance = true)
        {
            if (checkDistance && Vector3.Distance(transform.position, point) > _agent._earingDistance)
            {
                return;
            }

            _alertLevel = Mathf.Clamp01(_alertLevel + strength);
        }

        private void UpdateAlertLevel()
        {
            _alertSlider.value = _alertLevel;
        }

        private void Update()
        {
            if (GameManager.Instance.gameState == GameManager.GameState.Alert)
            {
                return;
            }

            if (_alertLevel > 0)
            {
                if (_alertLevel >= _maxAlertLevel)
                {
                    Debug.Log("Alarm triggered !");
                    // On d�clenche l'alarme
                    EventManager.OnChangeGameState(GameManager.GameState.Alert);

                    return;
                }

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
