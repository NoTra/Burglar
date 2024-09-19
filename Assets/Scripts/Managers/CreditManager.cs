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

        private void Start()
        {
            // First time enabled we load level
            InitLevelCredits();
        }

        private void OnEnable()
        {
            Debug.Log("ENABLE CREDITMANAGER");
            EventManager.CreditCollected += AddCredit;
            EventManager.LoadLevelEnd += InitLevelCredits;
            EventManager.LevelSuccess += LevelSuccess;
        }

        private void OnDisable()
        {
            EventManager.CreditCollected -= AddCredit;
            EventManager.LoadLevelEnd -= InitLevelCredits;
            EventManager.LevelSuccess -= LevelSuccess;
        }

        private void LevelSuccess()
        {
            // Exclude levels where minimum credits is 0
            if (minimumCredits == 0)
            {
                return;
            }
            
            // Add levelCredit - minimum to the player stash
            var amountToAdd = levelCredit - minimumCredits;
            Debug.Log("Adding " + amountToAdd + " (levelCredit = " + levelCredit + " & min = " + minimumCredits + ") to player stash (" + GameManager.Instance.credit + ")");
            GameManager.Instance.credit += amountToAdd;

            ResetLevelCredits();
        }

        private void InitLevelCredits()
        {
            if (LevelManager.Instance?._currentLevel == null)
            {
                // We hide the credit UI if the current level is null
                gameObject.SetActive(false);
                return;
            }

            // Find the current level
            var levelManager = LevelManager.Instance;
            if (levelManager._currentLevel == null)
            {
                levelManager.LoadLevelByIndex(levelManager._currentLevelIndex);
                return;
            }
            
            // Extract minimum credits
            minimumCredits = levelManager._currentLevel.minimumCredits;
            
            // Reset credit for next level
            if (levelManager._currentLevel.resetCredits)
            {
                GameManager.Instance.credit = 0;
            }
        }

        public void ResetLevelCredits()
        {
            levelCredit = 0;
            _creditSlider.value = 0;
            
            StartCoroutine(UpdateCreditUI());
        }
        
        /**
         * Initialize the credit slider
         */
        private void InitCreditSlider()
        {
            _creditSlider.maxValue = maximumCredits;
            _creditSlider.minValue = 0;
            _creditSlider.value = 0;
            levelCredit = 0;
            _creditText.text = levelCredit + " / " + maximumCredits;
            
            var t = (float)minimumCredits / maximumCredits;
            var x = Mathf.Lerp(-111f, 108f, t);

            if (minimumCredits != 0)
            {
                _minMarker.SetActive(true);
                // Move the min marker to represent the minimum credit (0 we put x at 0, 100% we put the x at 220, we lerp in between)
                _minMarker.transform.localPosition = new Vector3(x, 
                    _minMarker.transform.localPosition.y,
                    _minMarker.transform.localPosition.z);
            }
            else
            {
                // Hide the min marker if the minimum credit is 0
                _minMarker.SetActive(minimumCredits != 0);
            }
        }
        
        private IEnumerator UpdateCreditUI()
        {
            var previousCredit = _creditSlider.value;
            
            var targetCredit = levelCredit;
            
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
            _creditSlider.value = levelCredit;
            _creditText.text = levelCredit + " / " + maximumCredits;
        }
        
        private void AddCredit(int creditAmount)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundCredit);
            levelCredit += creditAmount;
            
            EventManager.OnUpdateObjectives();
            
            StartCoroutine(UpdateCreditUI());
        }

        public void SetMaxCredits(int maxCredits)
        {
            maximumCredits = maxCredits;
            if (!Instance.gameObject.activeSelf)
            {
                Instance.gameObject.SetActive(true);                
            }
            
            InitCreditSlider();
            StartCoroutine(UpdateCreditUI());
        }
    }
}
