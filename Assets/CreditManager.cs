using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using burglar.managers;
using TMPro;
using Unity.VisualScripting;

namespace burglar
{
    public class CreditManager : MonoBehaviour
    {
        private int levelCredit = 0;
        private int _minimumCredits;
        private int _maximumCredits;
        
        [SerializeField] private TextMeshProUGUI _creditText;
        [SerializeField] private Slider _creditSlider;

        private void OnEnable()
        {
            EventManager.CreditCollected += AddCredit;
        }

        private void Start()
        {
            _minimumCredits = LevelManager.Instance._currentLevelIndex * 100;
            
            var levelManager = LevelManager.Instance;
            var levelSO = levelManager.levels[levelManager._currentLevelIndex];

            InitCredit(levelSO.maximumCredits, levelSO.minimumCredits);
        }
        
        public void InitCredit(int maxCredit, int minCredit)
        {
            _creditSlider.maxValue = _maximumCredits;
            _creditSlider.minValue = _minimumCredits;
            _creditSlider.value = 0;
            levelCredit = 0;
            _creditText.text = levelCredit + " / " + _maximumCredits;
        }
        
        private void AddCredit(int credit)
        {
            levelCredit += credit;
            
            _creditSlider.value = levelCredit;
            _creditText.text = levelCredit + " / " + _maximumCredits;
        }
    }
}
