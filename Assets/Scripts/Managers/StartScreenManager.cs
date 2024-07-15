using burglar.persistence;
using TMPro;
using UnityEngine;

namespace burglar.managers
{
    public class StartScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private GameObject _newGameButton;
        /*[SerializeField] private GameObject _hudCanvas;
        [SerializeField] private GameObject _startScreenCanvas;
        
        [SerializeField] private GameObject _tutorialButton;
        [SerializeField] private GameObject _newGameButton;*/

        private void Start()
        {
            if(SaveLoadSystem.Instance.SaveExists() && _continueButton != null)
            {
                _continueButton.SetActive(true);
                var newGameText = _newGameButton.GetComponentInChildren<TextMeshProUGUI>();
                // Change size of newGameText
                newGameText.fontSize = 32;
            }
        }

        public void NewGameButton()
        {
            /*_startScreenCanvas.SetActive(false);
            _hudCanvas.SetActive(true);*/
            SaveLoadSystem.Instance.NewGame();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicLevel, false);
        }

        public void TutorialButton()
        {
            // Change music
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicTuto, false);
            
            LevelManager.Instance.LoadScene("intro");
        }

        public void LoadGameButton()
        {
            var lastSaveName = SaveLoadSystem.Instance.GetLastSaveName();
            Debug.Log("Loading game: " + lastSaveName);

            SaveLoadSystem.Instance.LoadGame(lastSaveName);
            Debug.Log("Game loaded !");
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
