using System;
using UnityEngine;
using burglar.managers;
using System.Collections;

namespace burglar
{
    public class PauseScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _returnMainMenuButton;
        [SerializeField] private GameObject _settingsButton;
        [SerializeField] private GameObject _exitGameButton;
        
        [SerializeField] private RectTransform _pauseMenuCanvas;
        [SerializeField] private RectTransform _settingsCanvas;
        
        private Vector2 _pauseMenuStartPosition;
        private Vector2 _settingsStartPosition;
        
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float _animationDuration = 0.5f;

        private float screenWidth;
        
        private SettingsManager _settingsManager;

        private void OnEnable()
        {
            EventManager.TogglePause += OnTogglePause;
        }
        
        private void OnDisable()
        {
            EventManager.TogglePause -= OnTogglePause;
        }

        private void OnTogglePause()
        {
            // If we are in settings, we just want to go back
            if (_settingsCanvas.gameObject.activeSelf)
            {
                _settingsManager._backButton.onClick.Invoke();
            }
        }

        private void Start()
        {
            EventManager.TogglePause += OnTogglePause;
            
            screenWidth = Screen.width;
            
            _settingsCanvas.anchoredPosition = _settingsStartPosition + new Vector2(screenWidth, 0);
            
            _settingsStartPosition = _settingsCanvas.anchoredPosition;
            _pauseMenuStartPosition = _pauseMenuCanvas.anchoredPosition;
            
            _settingsManager = _settingsCanvas.GetComponent<SettingsManager>(); 
            _settingsManager._backButton.onClick.AddListener(() =>
            {
                StartCoroutine(SlideInSettings(
                    _pauseMenuStartPosition - new Vector2(screenWidth, 0),
                    _pauseMenuStartPosition,
                    _settingsStartPosition - new Vector2(screenWidth, 0),
                    _settingsStartPosition,
                    false
                ));
            });
        }
        
        public RectTransform GetSettingsCanvas()
        {
            return _settingsCanvas;
        }

        public void ExitGameButton()
        {
            Application.Quit();
        }
        
        public void ReturnMainMenuButton()
        {
            // Remove HUD
            // UIManager.Instance.HUD.SetActive(false);
            DialogManager.Instance.DialogPanel.SetActive(false);
            // UIManager.Instance.ToggleHudVisibility();
            
            // Remove pause
            GameManager.Instance.TogglePause();
            
            LevelManager.Instance.LoadMainMenu();
        }
        
        public void SettingsButton()
        {
            _settingsCanvas.gameObject.SetActive(true);
            
            StartCoroutine(SlideInSettings(
                _pauseMenuStartPosition,
                _pauseMenuStartPosition - new Vector2(screenWidth, 0),
                _settingsStartPosition,
                _settingsStartPosition - new Vector2(screenWidth, 0),
                true
            ));
        }
        
        private IEnumerator SlideInSettings(Vector2 startMenuFrom, Vector2 startMenuTo, Vector2 settingsFrom, Vector2 settingsTo, bool setActiveValue = true)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundSwoosh);
            
            var elapsedTime = 0f;

            while (elapsedTime < _animationDuration)
            {
                _pauseMenuCanvas.anchoredPosition = Vector2.LerpUnclamped(startMenuFrom, startMenuTo, _animationCurve.Evaluate(elapsedTime / _animationDuration));
                _settingsCanvas.anchoredPosition = Vector2.LerpUnclamped(settingsFrom, settingsTo, _animationCurve.Evaluate(elapsedTime / _animationDuration));
                elapsedTime += Time.fixedUnscaledDeltaTime;
                
                yield return null;
            }

            _pauseMenuCanvas.anchoredPosition = startMenuTo;
            _settingsCanvas.anchoredPosition = settingsTo;
            
            _settingsCanvas.gameObject.SetActive(setActiveValue);
        }
        
        public void ResumeButton()
        {
            GameManager.Instance.TogglePause();
        }
    }
}
