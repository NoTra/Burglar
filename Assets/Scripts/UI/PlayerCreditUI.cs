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
            creditText.text = "+" + amount;
            creditText.enabled = true;
            
            // Launch animation : reduce Y to 0 and grow scale to 1.5 and make it slowly disappear
            StartCoroutine(AnimateCreditCollected());
        }

        private IEnumerator AnimateCreditCollected()
        {
            float time = 0;
            var duration = 1f;
            
            var initialScale = creditText.rectTransform.localScale;
            var targetScale = new Vector3(2f, 2f, 2f);
            
            var initialPosition = creditText.rectTransform.position;
            var targetPosition = new Vector3(initialPosition.x, initialPosition.y + 100, initialPosition.z);
            
            var initialColor = creditText.color;
            var targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
            
            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration; 
                
                creditText.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                creditText.rectTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                creditText.color = Color.Lerp(initialColor, targetColor, t);
                
                yield return null;
            }
            
            creditText.enabled = false;
            creditText.transform.localScale = initialScale;
            creditText.transform.position = initialPosition;
            creditText.color = initialColor;
            
        }
    }
}
