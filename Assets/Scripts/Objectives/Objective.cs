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
        public bool _objectiveCompleted = false;
        
        public TextMeshProUGUI _objectiveTextComponent;
        public Toggle _objectiveToggle;

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
            if (_objectiveCompleted)
            {
                return;
            }
            
            var currentState = _objectiveToggle.isOn;
            var newState = value.Invoke();
            
            if (newState == currentState)
            {
                return;
            }
            
            EventManager.OnObjectiveCompleted(this);
            Debug.Log("levelCredit(" + CreditManager.Instance.levelCredit + ") >= minimumCredits( " + LevelManager.Instance._currentLevel.minimumCredits + ") : " + value.Invoke());
            
            // Hide checkmark from Toggle
            var checkmark = _objectiveToggle.GetComponentsInChildren<Image>()[1];
            checkmark.enabled = false;
            
            _objectiveToggle.isOn = value.Invoke();
            _objectiveCompleted = true;
            
            if (_objectiveToggle.isActiveAndEnabled == false) return;

            StartCoroutine(AnimateCheckmarkCoroutine(checkmark));
        }

        private IEnumerator AnimateCheckmarkCoroutine(Image checkmark)
        {
            var elapsedTime = 0f;
            var duration = 0.5f;
            var startScale = new Vector3(4f, 4f, 4f);
            var targetScale = checkmark.transform.localScale;

            checkmark.transform.localScale = startScale;
            checkmark.enabled = true;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                checkmark.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
                yield return null;
            }
            
            checkmark.transform.localScale = targetScale;
        }
    }
}
