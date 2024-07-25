using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

using burglar.managers;

namespace burglar
{
    public class CreditManager : MonoBehaviour
    {
        public int levelCredit = 0;
        public int minimumCredits;
        public int maximumCredits;
        
        [SerializeField] private TextMeshProUGUI _creditText;
        [SerializeField] private Slider _creditSlider;
        
        [SerializeField] private GameObject _minMarker;
        
        private static CreditManager instance = null;
        
        public static CreditManager Instance => instance;
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            EventManager.CreditCollected += AddCredit;
            EventManager.LoadLevel += OnLoadLevel;
            
            // First time enabled we load level
            OnLoadLevel();
        }
        
        private void OnDisable()
        {
            EventManager.CreditCollected -= AddCredit;
            EventManager.LoadLevel -= OnLoadLevel;
        }
        
        private void OnLoadLevel()
        {
            if (LevelManager.Instance?._currentLevel == null)
            {
                Debug.LogError("Current level is null");
                return;
            }

            var levelManager = LevelManager.Instance;
            minimumCredits = levelManager._currentLevel.minimumCredits;
            maximumCredits = levelManager._currentLevel.maximumCredits;
            
            if (levelManager._currentLevel.resetCredits)
            {
                GameManager.Instance.credit = 0;
            }
            
            InitCredit();
        }
        
        private void InitCredit()
        {
            _creditSlider.maxValue = maximumCredits;
            _creditSlider.minValue = 0;
            _creditSlider.value = 0;
            levelCredit = 0;
            _creditText.text = levelCredit + " / " + maximumCredits;
            
            var t = (float)minimumCredits / maximumCredits;
            var x = Mathf.Lerp(-111f, 108f, t);
            
            // Move the min marker to represent the minimum credit (0 we put x at 0, 100% we put the x at 220, we lerp in between)
            _minMarker.transform.localPosition = new Vector3(x, 
                _minMarker.transform.localPosition.y,
                _minMarker.transform.localPosition.z);
            
            // Hide the min marker if the minimum credit is 0
            _minMarker.SetActive(minimumCredits != 0);
        }
        
        private IEnumerator UpdateCreditUI(int amount)
        {
            var audioSource = AudioManager.Instance.PlaySFX(AudioManager.Instance.soundCredit);
            var previousCredit = levelCredit;
            var targetCredit = previousCredit + amount;
            
            var elapsedTime = 0f;
            var duration = 1f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                
                var newCredit = Mathf
                    .Ceil(Mathf.Lerp(previousCredit, targetCredit, elapsedTime / duration));
                
                _creditSlider.value = newCredit;
                _creditText.text = newCredit + " / " + maximumCredits;
                
                yield return null;
            }
            
            // Level credits
            levelCredit = targetCredit;
            _creditSlider.value = levelCredit;
            _creditText.text = levelCredit + " / " + maximumCredits;
            
            // Global credits
            GameManager.Instance.credit += amount;
            
            audioSource.Stop();
        }
        
        private void AddCredit(int creditAmount)
        {
            StartCoroutine(UpdateCreditUI(creditAmount));
            
            levelCredit += creditAmount;
        }
    }
}
