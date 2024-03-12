using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace burglar
{

    public class UIManager : MonoBehaviour
    {
        // GameOver 💀
        [Header("GameOver")]
        [SerializeField] private GameObject UIGameOverPanel;

        // Credit HUD 💰
        [Header("CreditHUD")]
        [SerializeField] private TextMeshProUGUI UICreditTMP;

        // Safe UI 🔓
        [Header("SafeUI")]
        [SerializeField] private GameObject UISafePanel;
        [SerializeField] private TextMeshProUGUI _combinationText;
        [SerializeField] private GridLayoutGroup _gridContainer;
        [SerializeField] private GameObject _buttonPrefab;
        private List<CombinationButton> _buttons = new List<CombinationButton>();
        public Color _defaultColor = new Color(204, 255, 204, 1);
        public Color _hoverColor = new Color(153, 255, 153, 1);
        public Color _selectedColor = new Color(51, 204, 51, 1);
        public Color _disabledColor = new Color(242, 242, 242, 1);

        // Outline
        [Header("Outline")]
        public Color OutlineColor = Color.yellow;
        public float OutlineWidth = 3f;

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
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            EventManager.PlayerCaught += (player) => OnPlayerCaught(player);

            EventManager.CreditCollected += (amount) => OnCreditCollected(amount);

            EventManager.OpenSafe += (safe) => OpenSafeUI(safe);

            EventManager.SuccessSafeCrack += (safe) => OnSuccessSafeCrack(safe);

            EventManager.FailSafeCrack += () => OnFailSafeCrack();
        }

        private void OnDisable()
        {
            EventManager.PlayerCaught -= (player) => OnPlayerCaught(player);

            EventManager.CreditCollected -= (amount) => OnCreditCollected(amount);

            EventManager.OpenSafe -= (safe) => OpenSafeUI(safe);

            EventManager.SuccessSafeCrack -= (safe) => OnSuccessSafeCrack(safe);

            EventManager.FailSafeCrack -= () => OnFailSafeCrack();
        }

        #region PlayerCaught
        private void OnPlayerCaught(GameObject player)
        {
            UIGameOverPanel.SetActive(true);

            StartCoroutine(BeforeRestartWaitFor(3f));
        }

        private IEnumerator BeforeRestartWaitFor(float durationInSeconds)
        {
            yield return new WaitForSeconds(durationInSeconds);

            // Stop All Coroutines
            StopAllCoroutines();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        #endregion

        #region CreditCollected
        private void OnCreditCollected(int amount)
        {

            StartCoroutine(UpdateCreditUI(amount));
        }

        private IEnumerator UpdateCreditUI(int amount)
        {
            for (int i = GameManager.Instance.credit; i <= GameManager.Instance.credit + amount; i++)
            {
                UICreditTMP.text = i.ToString();
                yield return null;
            }

            GameManager.Instance.credit += amount;

            UICreditTMP.text = GameManager.Instance.credit.ToString();
        }

        #endregion

        #region SafeUI

        private void OpenSafeUI(Safe safe)
        {
            ClearSafe();
            UISafePanel.SetActive(true);

            Time.timeScale = 0;

            _combinationText.text = "Combination : " + safe.GetStringCombination();

            var nbButtonsByRow = safe.GetLevel() + 1;

            // Set up Grid Layout Group to display nbButtonsByRow buttons per row
            _gridContainer.constraintCount = nbButtonsByRow;

            // Create buttons for each number in the matrix
            for (int i = 0; i < nbButtonsByRow; i++)
            {
                for (int j = 0; j < nbButtonsByRow; j++)
                {
                    var button = Instantiate(_buttonPrefab, _gridContainer.transform);
                    var combinationButton = button.GetComponent<CombinationButton>();
                    combinationButton.Coordinates = new Vector2((float) i, (float) j);
                    combinationButton.Safe = safe;
                    button.GetComponentInChildren<TextMeshProUGUI>().text = safe.GetMatrix()[i, j].ToString();

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
            if (safe.GetSelectedCombinationLength() > 0)
            {
                // Disable all buttons
                foreach (var button in _buttons)
                {
                    button._isClickable = false;
                }
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
                combinationButton._isClickable = true;
            }

            if (southDirection.y <= safe.GetLevel() && !safe.GetSelectedCombination().Contains(southDirection))
            {
                // South is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)southDirection.x, (int)southDirection.y);
                combinationButton._isClickable = true;
            }

            if (eastDirection.x <= safe.GetLevel() && !safe.GetSelectedCombination().Contains(eastDirection))
            {
                // East is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)eastDirection.x, (int)eastDirection.y);
                combinationButton._isClickable = true;
            }

            if (westDirection.x >= 0 && !safe.GetSelectedCombination().Contains(westDirection))
            {
                // West is clickable
                var combinationButton = FindCombinationButtonByCoordinates((int)westDirection.x, (int)westDirection.y);
                combinationButton._isClickable = true;
            }

            foreach (var combinationButton in _buttons)
            {
                // Change background colors
                combinationButton._background.color = ((combinationButton._isSelected) ? UIManager.Instance._selectedColor : ((combinationButton._isClickable) ? UIManager.Instance._defaultColor : UIManager.Instance._disabledColor));
            }
        }

        private void OnSuccessSafeCrack(Safe safe)
        {
            // Timescale 1
            Time.timeScale = 1;

            UISafePanel.SetActive(false);
        }

        private void OnFailSafeCrack()
        {
            UISafePanel.SetActive(false);
        }


        private CombinationButton FindCombinationButtonByCoordinates(int x, int y)
        {
            return _buttons.Find(button => button.Coordinates.x == x && button.Coordinates.y == y);
        }

        #endregion
    }
}
