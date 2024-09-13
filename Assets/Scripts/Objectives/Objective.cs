using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using burglar.managers;

namespace burglar
{
    public class Objective : MonoBehaviour
    {
        public int _objectiveIndex;
        public string _objectiveText;
        public bool _objectiveValue;
        
        public TextMeshProUGUI _objectiveTextComponent;
        public Toggle _objectiveToggle;

        private void OnEnable()
        {
            EventManager.PlayerCaught += ResetObjective;
        }

        private void OnDisable()
        {
            EventManager.PlayerCaught -= ResetObjective;
        }

        private void ResetObjective(GameObject arg0)
        {
            _objectiveToggle.isOn = false;
            var checkmark = _objectiveToggle.GetComponentsInChildren<Image>()[1];
            checkmark.enabled = false;
        }

        public void SetObjective(int index, string objectiveKey, bool objectiveValue)
        {
            _objectiveIndex = index;
            _objectiveText = objectiveKey;
            _objectiveValue = objectiveValue;
            
            _objectiveTextComponent.text = objectiveKey;
            _objectiveToggle.isOn = objectiveValue;
        }
        
        public void UpdateObjective(Func<bool> value)
        {
            Debug.Log("UpdateObjective called");
            
            var currentState = _objectiveToggle.isOn;
            var newState = value.Invoke();
            
            // Don't want to send ObjectiveCompleted event if the state is the same
            if (newState == currentState)
            {
                return;
            }
            
            EventManager.OnObjectiveCompleted(this);
            
            var checkmark = _objectiveToggle.GetComponentsInChildren<Image>()[1];
            // Hide checkmark from Toggle
            checkmark.enabled = false;
            
            _objectiveToggle.isOn = value.Invoke();
            
            Debug.Log("Pour label : " + _objectiveText + " la valeur est : " + _objectiveToggle.isOn);
            
            if (_objectiveToggle.isActiveAndEnabled == false) return;
            if (_objectiveToggle.isOn == false) return;

            Debug.Log("Animate checkmark routine");
            StartCoroutine(AnimateCheckmarkCoroutine(checkmark));
        }

        private IEnumerator AnimateCheckmarkCoroutine(Image checkmark)
        {
            var elapsedTime = 0f;
            var duration = 0.5f;
            var startScale = new Vector3(4f, 4f, 4f);
            var targetScale = new Vector3(1f, 1f, 1f);

            checkmark.transform.localScale = startScale;
            checkmark.enabled = true;
            
            while (elapsedTime < duration)
            {
                checkmark.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            checkmark.transform.localScale = targetScale;
        }
    }
}
