using burglar.persistence;
using TMPro;
using UnityEngine;
using System.Collections;
using EasyTransition;
using UnityEngine.SceneManagement;

namespace burglar.managers
{
    public class StartScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private GameObject _newGameButton;

        [SerializeField] private RectTransform _startMenuCanvas;
        [SerializeField] private RectTransform _settingsCanvas;
        
        private Vector2 _startMenuStartPosition;
        private Vector2 _settingsStartPosition;
        
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float _animationDuration = 0.5f;

        private float screenWidth;
        
        private SettingsManager _settingsManager;

        private void Awake()
        {
            _startMenuCanvas = GetComponent<RectTransform>();
        }

        private void Start()
        {
            UIManager.Instance.HUD.SetActive(false);
            
            var audioManager = AudioManager.Instance;
            // If a music is playing, stop it
            if (audioManager.musicAudioSource.isPlaying)
            {
                audioManager.musicAudioSource.Stop();
            }
            
            if (audioManager.musicAudioSource2.isPlaying)
            {
                audioManager.musicAudioSource2.Stop();
            }
            
            audioManager.PlayMusic(AudioManager.Instance.musicMenu, false, false);
            
            
            screenWidth = Screen.width;
            _settingsCanvas.anchoredPosition = _settingsStartPosition + new Vector2(screenWidth, 0);
            
            _settingsStartPosition = _settingsCanvas.anchoredPosition;
            _startMenuStartPosition = _startMenuCanvas.anchoredPosition;
            
            if(SaveLoadSystem.Instance.SaveExists() && _continueButton != null)
            {
                _continueButton.SetActive(true);
                var newGameText = _newGameButton.GetComponentInChildren<TextMeshProUGUI>();
                // Change size of newGameText
                newGameText.fontSize = 32;
            }
            
            _settingsManager = _settingsCanvas.GetComponent<SettingsManager>(); 
            _settingsManager._backButton.onClick.AddListener(() =>
            {
                StartCoroutine(SlideInSettings(
                    _startMenuStartPosition - new Vector2(screenWidth, 0),
                    _startMenuStartPosition,
                    _settingsStartPosition - new Vector2(screenWidth, 0),
                    _settingsStartPosition,
                    false
                ));
            });
        }

        public void NewGameButton()
        {
            /*_startScreenCanvas.SetActive(false);
            _hudCanvas.SetActive(true);*/
            SaveLoadSystem.Instance.NewGame();
            
            Debug.Log("Start transition");
            TransitionManager.Instance().Transition("StartCinematic", UIManager.Instance.fadeTransition, 0f);
            
            // Stop current music
            StartCoroutine(AudioManager.Instance.FadeOut(AudioManager.Instance.musicAudioSource, 0.5f));
            
            // Load scene with SceneManager
            // SceneManager.LoadScene("StartCinematic");
        }

        public void TutorialButton()
        {
            LevelManager.Instance.LoadScene("intro");
        }

        public void LoadGameButton()
        {
            var lastSaveName = SaveLoadSystem.Instance.GetLastSaveName();

            SaveLoadSystem.Instance.LoadGame(lastSaveName);
        }

        public void SettingsButton()
        {
            // gameObject.SetActive(false);
            // Position settingsCanvas at the right of the screen 
            _settingsCanvas.gameObject.SetActive(true);
            
            StartCoroutine(SlideInSettings(
                _startMenuStartPosition,
                _startMenuStartPosition - new Vector2(screenWidth, 0),
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
                _startMenuCanvas.anchoredPosition = Vector2.LerpUnclamped(startMenuFrom, startMenuTo, _animationCurve.Evaluate(elapsedTime / _animationDuration));
                _settingsCanvas.anchoredPosition = Vector2.LerpUnclamped(settingsFrom, settingsTo, _animationCurve.Evaluate(elapsedTime / _animationDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _startMenuCanvas.anchoredPosition = startMenuTo;
            _settingsCanvas.anchoredPosition = settingsTo;
            
            _settingsCanvas.gameObject.SetActive(setActiveValue);
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        private void Update()
        {
            // If esc is pressed, Invoke click on back button
            if (Input.GetKeyUp(KeyCode.Escape) && _settingsCanvas.gameObject.activeSelf)
            {
                _settingsManager._backButton.onClick.Invoke();
            }
        }
    }
}
