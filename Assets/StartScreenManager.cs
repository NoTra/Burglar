using burglar.persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class StartScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _hudCanvas;
        [SerializeField] private GameObject _startScreenCanvas;
        [SerializeField] private GameObject _continueButton;

        private void Start()
        {
            _continueButton.SetActive(SaveLoadSystem.Instance.SaveExists());
        }

        public void NewGameButton()
        {
            _startScreenCanvas.SetActive(false);
            _hudCanvas.SetActive(true);
            SaveLoadSystem.Instance.NewGame();
        }

        public void LoadGameButton()
        {
            Debug.Log("Load game !");
        }

        public void OptionsButton()
        {
            Debug.Log("Options !");
        }

        public void QuitButton()
        {
            Debug.Log("Quit !");
            Application.Quit();
        }
    }
}
