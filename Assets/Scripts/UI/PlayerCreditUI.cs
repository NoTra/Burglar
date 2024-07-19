using System;
using System.Collections;
using TMPro;
using UnityEngine;

using burglar.managers;

namespace burglar
{
    public class PlayerCreditUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditText;
        
        // Start is called before the first frame update
        private void OnEnable()
        {
            EventManager.CreditCollected += LaunchCreditCollectedAnimation;
        }
        
        private void OnDisable()
        {
            EventManager.CreditCollected -= LaunchCreditCollectedAnimation;
        }

        private void LaunchCreditCollectedAnimation(int amount)
        {
            if (!creditText) return;
            
            // Change creditText to amount
            creditText.text = "+" + amount.ToString();
            creditText.enabled = true;
            
            // Launch animation : reduce Y to 0 and grow scale to 1.5 and make it slowly disappear
            StartCoroutine(AnimateCreditCollected());
        }

        private IEnumerator AnimateCreditCollected()
        {
            float time = 0;
            var initialScale = creditText.transform.localScale;
            var initialPosition = creditText.transform.position;
            var targetScale = new Vector3(2f, 2f, 2f);
            var targetPosition = new Vector3(initialPosition.x, initialPosition.y + 100, initialPosition.z);
            var initialOpacity = 1;
            var targetOpacity = 0;
            
            while (time < 1)
            {
                time += Time.deltaTime;
                creditText.transform.localScale = Vector3.Lerp(initialScale, targetScale, time);
                creditText.transform.position = Vector3.Lerp(initialPosition, targetPosition, time);
                
                var color = creditText.color;
                color.a = Mathf.Lerp(initialOpacity, targetOpacity, time);
                creditText.color = color;
                
                yield return null;
            }
            creditText.enabled = false;
            creditText.transform.localScale = initialScale;
            creditText.transform.position = initialPosition;
            
        }
    }
}
