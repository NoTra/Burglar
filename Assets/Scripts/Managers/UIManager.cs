using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using burglar.environment;
using burglar.objectives;
using burglar.UI;
using EasyTransition;

namespace burglar.managers
{

    public class UIManager : MonoBehaviour
    {
        [Header("UIManager")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject eventSystem;

        // Screens 🚪
        [Header("Screens")]
        public GameObject StartScreen;
        public GameObject PauseScreen;

        // HUD 💰
        [Header("HUD")]
        public GameObject HUD;
        [SerializeField] private GameObject UIInteractIcon;
        [SerializeField] private TextMeshProUGUI UICreditTMP;
        public CreditManager creditManager;
        public GameObject UITitle;
        public TextMeshProUGUI UICreditText;
        
        [Header("Objectives")]
        public GameObject UIObjective;
        public GameObject objectivesContainer;
        public GameObject objectivePrefab;

        // GameOver 💀
        [Header("GameOver")]
        [SerializeField] private GameObject UIGameOverPanel;

        // Safe UI 🔓
        [Header("SafeUI")]
        [SerializeField] private GameObject UISafePanel;
        [SerializeField] private TextMeshProUGUI _combinationText;
        [SerializeField] private GridLayoutGroup _gridContainer;
        [SerializeField] private GameObject _buttonPrefab;
        private List<CombinationButton> _buttons = new List<CombinationButton>();
        public Color _defaultColor = new(204, 255, 204, 1);
        public Color _defaultBGColor = new(204, 255, 204, 1);
        public Color _availableChoiceColor = new(255, 255, 255, 1);
        public Color _availableChoiceBGColor = new(255, 255, 255, 1);
        public Color _hoverColor = new(153, 255, 153, 1);
        public Color _hoverBGColor = new(153, 255, 153, 1);
        public Color _selectedColor = new(51, 204, 51, 1);
        public Color _selectedBGColor = new(51, 204, 51, 1);
        public Color _disabledColor = new(242, 242, 242, 1);
        public Color _disabledBGColor = new(242, 242, 242, 1);

        [Header("Transitions")]
        public TransitionSettings caughtTransition;
        public TransitionSettings levelTransition;
        public TransitionSettings fadeTransition;
        
        
        // Singleton
        private static UIManager instance = null;
        public static UIManager Instance => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
        
            instance = this;
            // if gameObject already exists in DontDestroyOnLoad, we can destroy it 
            DontDestroyOnLoad(gameObject);

            // Add canvas to DontDestroyOnLoad
            if (canvas != null)
            {
                DontDestroyOnLoad(canvas.gameObject);
            }

            if (eventSystem != null)
            {
                DontDestroyOnLoad(eventSystem.gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.OpenSafe += (safe) => OpenSafeUI(safe);
            EventManager.SuccessSafeCrack += (safe) => OnSuccessSafeCrack(safe);
            EventManager.FailSafeCrack += (safe) => OnFailSafeCrack(safe);
            EventManager.EnterInteractibleArea += (interactible) => OnEnterInteractibleArea(interactible);
            EventManager.Interact += (interactible) => OnInteract(interactible);
            EventManager.ExitInteractibleArea += (interactible) => OnExitInteractibleArea(interactible);
            EventManager.TogglePause += () => OnTogglePause();
            EventManager.CinematicEnd += ShowTitle;
            EventManager.CreditCollected += (credit) => LaunchCreditCollectedAnimation(credit);
        }

        public void HideHUD()
        {
            HUD.SetActive(false);
            DialogManager.Instance.DialogPanel.SetActive(false);
        }
        
        private void LaunchCreditCollectedAnimation(int amount)
        {
            if (!UICreditText) return;
            
            // Change creditText to amount
            UICreditText.text = amount.ToString();
            UICreditText.enabled = true;
            
            // Launch animation : reduce Y to 0 and grow scale to 1.5 and make it slowly disappear
            StartCoroutine(AnimateCreditCollected());
        }

        private IEnumerator AnimateCreditCollected()
        {
            var time = 0f;
            var duration = 1f;
            
            var initialScale = UICreditText.rectTransform.localScale;
            var targetScale = new Vector3(2f, 2f, 2f);
            
            var initialPosition = UICreditText.rectTransform.position;
            var targetPosition = new Vector3(initialPosition.x, initialPosition.y + 100, initialPosition.z);
            
            var initialColor = UICreditText.color;
            var targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
            
            // Go up, grow and disappear slowly
            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration; 
                
                UICreditText.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                UICreditText.rectTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
                UICreditText.color = Color.Lerp(initialColor, targetColor, t);
                
                yield return null;
            }
            
            UICreditText.enabled = false;
            UICreditText.transform.localScale = initialScale;
            UICreditText.transform.position = initialPosition;
            UICreditText.color = initialColor;
        }

        private void OnDisable()
        {
            EventManager.OpenSafe -= (safe) => OpenSafeUI(safe);
            EventManager.SuccessSafeCrack -= (safe) => OnSuccessSafeCrack(safe);
            EventManager.FailSafeCrack -= (safe) => OnFailSafeCrack(safe);
            EventManager.EnterInteractibleArea -= (interactible) => OnEnterInteractibleArea(interactible);
            EventManager.Interact -= (interactible) => OnInteract(interactible);
            EventManager.ExitInteractibleArea -= (interactible) => OnExitInteractibleArea(interactible);
            EventManager.TogglePause -= OnTogglePause;
            EventManager.CinematicEnd -= ShowTitle;
            // ReSharper disable once EventUnsubscriptionViaAnonymousDelegate
            EventManager.CreditCollected -= LaunchCreditCollectedAnimation;
        }

        private void ShowTitle()
        {
            if (LevelManager.Instance._currentLevel == null) LevelManager.Instance.LoadNextLevel();

            var levelTitle = LevelManager.Instance._currentLevel.levelName;
            
            // Update title
            UITitle.GetComponentInChildren<TextMeshProUGUI>().text = levelTitle;
            UITitle.SetActive(true);
            
            StartCoroutine(ShowHud());
            // ToggleHudVisibility();
        }
        
        // private IEnumerator WaitAndShowTitle(float durationInSeconds)
        // {
        //     Debug.Log("UITitle.SetActive(true);");
        //     yield return new WaitForSeconds(durationInSeconds);
        //     
        // }
        
        public void HideHud()
        {
            HUD.SetActive(false);
        }
        
        public void ToggleHudVisibility()
        {
            var go = HUD.gameObject;
            // Make the gameObject appear slowly or disappear slowly
            if (go.activeSelf)
            {
                StartCoroutine(FadeOutHud(go));
            }
            else
            {
                StartCoroutine(FadeInHud(go));
            }
        }
        
        private IEnumerator ShowHud()
        {
            yield return new WaitForSeconds(4f);
            
            // Make the gameObject appear slowly or disappear slowly
            StartCoroutine(FadeInHud(HUD.gameObject));
        }
        
        private IEnumerator FadeOutHud(GameObject go)
        {
            // Get the CanvasGroup component of the gameObject
            var canvasGroup = go.GetComponent<CanvasGroup>();
            
            // While the alpha of the CanvasGroup is greater than 0
            while (canvasGroup.alpha > 0)
            {
                // Decrease the alpha of the CanvasGroup
                canvasGroup.alpha -= Time.fixedDeltaTime;
                
                // Wait for the end of the frame
                yield return null;
            }
            
            // Deactivate the gameObject
            go.SetActive(false);
        }
        
        private IEnumerator FadeInHud(GameObject go)
        {
            // Get the CanvasGroup component of the gameObject
            var canvasGroup = go.GetComponent<CanvasGroup>();
            
            // Activate the gameObject
            go.SetActive(true);
            
            canvasGroup.alpha = 0;
            
            float duration = 1f;
            float elapsedTime = 0f;
            
            // While the alpha of the CanvasGroup is less than 1
            while (elapsedTime < duration)
            {
                // Increase the alpha of the CanvasGroup
                canvasGroup.alpha += elapsedTime / duration;
                
                elapsedTime += Time.fixedDeltaTime;
                
                // Wait for the end of the frame
                yield return null;
            }
            
            canvasGroup.alpha = 1;
            go.SetActive(true);
        }

        #region SafeUI

        private void OpenSafeUI(Safe safe)
        {
            Debug.Log("OpenSafeUI called !");
            ClearSafe();

            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundSwoosh);
            
            UISafePanel.SetActive(true);

            Time.timeScale = 0;

            _combinationText.text = safe.GetStringCombination();

            var nbButtonsByRow = safe.GetLevel() + 1;

            if (GameManager.Instance.GetSelectedItemSlug() == "unlocker")
            {
                nbButtonsByRow--;
            }

            // Set up Grid Layout Group to display nbButtonsByRow buttons per row
            _gridContainer.constraintCount = nbButtonsByRow;

            // Create buttons for each number in the matrix
            for (var i = 0; i < nbButtonsByRow; i++)
            {
                for (var j = 0; j < nbButtonsByRow; j++)
                {
                    var button = Instantiate(_buttonPrefab, _gridContainer.transform);
                    var combinationButton = button.GetComponent<CombinationButton>();
                    combinationButton.Coordinates = new Vector2((float) i, (float) j);
                    combinationButton.Safe = safe;
                    combinationButton._text.text = safe.GetMatrix()[i, j].ToString();

                    _buttons.Add(combinationButton);
                }
            }
        }

        private void ClearSafe()
        {
            // Clear all _gridContainer children
            foreach (Transform child in _gridContainer.transform)
            {
                Destroy(child.gameObject);
            }

            _buttons.Clear();
        }

        public void UpdateSafeGrid(Safe safe, Vector2 lastClickedCoordinates)
        {
            if (safe.GetSelectedCombinationLength() == 0)
            {
                foreach (var button in _buttons)
                {
                    button._background.color = UIManager.Instance._defaultBGColor;
                    button._text.color = UIManager.Instance._defaultColor;
                }
                return;
            }
            
            // Disable all buttons
            foreach (var button in _buttons)
            {
                button.isClickable = false;
            }

            // First define if the buttons north, south, east and west of lastClickedCoordinates are possible to click
            var northDirection = new Vector2(lastClickedCoordinates.x, lastClickedCoordinates.y - 1);
            var southDirection = new Vector2(lastClickedCoordinates.x, lastClickedCoordinates.y + 1);
            var eastDirection = new Vector2(lastClickedCoordinates.x + 1, lastClickedCoordinates.y);
            var westDirection = new Vector2(lastClickedCoordinates.x - 1, lastClickedCoordinates.y);

            if (northDirection.y >= 0 && !safe.GetSelectedCombination().Contains(northDirection))
            {
                // North is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)northDirection.x, (int)northDirection.y);
                combinationButton.isClickable = true;
            }

            if (southDirection.y <= safe.GetLevel() && !safe.GetSelectedCombination().Contains(southDirection))
            {
                // South is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)southDirection.x, (int)southDirection.y);
                combinationButton.isClickable = true;
            }

            if (eastDirection.x <= safe.GetLevel() && !safe.GetSelectedCombination().Contains(eastDirection))
            {
                // East is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)eastDirection.x, (int)eastDirection.y);
                combinationButton.isClickable = true;
            }

            if (westDirection.x >= 0 && !safe.GetSelectedCombination().Contains(westDirection))
            {
                // West is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)westDirection.x, (int)westDirection.y);
                combinationButton.isClickable = true;
            }

            foreach (var combinationButton in _buttons)
            {
                // Change background colors
                
                combinationButton._background.color = (
                    (combinationButton.isSelected) ? UIManager.Instance._selectedBGColor : (
                        (combinationButton.isClickable) ? UIManager.Instance._availableChoiceBGColor : UIManager.Instance._disabledBGColor)
                    );
                combinationButton._text.color = (
                    (combinationButton.isSelected) ? UIManager.Instance._selectedColor : (
                        (combinationButton.isClickable) ? UIManager.Instance._availableChoiceColor : UIManager.Instance._disabledColor)
                    );
            }
        }

        private void OnSuccessSafeCrack(Safe safe)
        {
            // Timescale 1
            Time.timeScale = 1;

            UISafePanel.SetActive(false);
        }

        private void OnFailSafeCrack(Safe safe)
        {
            CloseSafeUI();
        }

        public void CloseSafeUI()
        {
            UISafePanel.SetActive(false);
            
            EventManager.OnCloseSafe();
            
            Time.timeScale = 1;
        }


        private CombinationButton FindCombinationButtonByCoordinates(int x, int y)
        {
            return _buttons.Find(button => button.Coordinates.x == x && button.Coordinates.y == y);
        }

        #endregion
        
        #region Interactible
        private void OnEnterInteractibleArea(Interactible interactible)
        {
            UIInteractIcon.SetActive(true);
        }
        
        private void OnInteract(Interactible interactible)
        {
            UIInteractIcon.SetActive(false);
        }
        
        private void OnExitInteractibleArea(Interactible interactible)
        {
            UIInteractIcon.SetActive(false);
        }
        #endregion
        
        #region Pause
        
        private void OnTogglePause()
        {
            if (PauseScreen.activeSelf && PauseScreen.GetComponent<PauseScreenManager>().GetSettingsCanvas().gameObject.activeSelf)
            {
                return;
            }
            
            // Do not show HUD when pause screen is active
            HUD.SetActive(PauseScreen.activeSelf);
            
            // Activate pause screen
            PauseScreen.SetActive(!PauseScreen.activeSelf);
        }
        
        #endregion

        public GameObject GetCanvasGO()
        {
            return canvas;
        }
    }
}
