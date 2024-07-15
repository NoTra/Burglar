using UnityEngine;
using burglar.managers;

namespace burglar
{
    public class PauseScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject _returnMainMenuButton;
        [SerializeField] private GameObject _settingsButton;
        [SerializeField] private GameObject _exitGameButton;

        public void ExitGameButton()
        {
            Application.Quit();
        }
        
        public void ReturnMainMenuButton()
        {
            // Remove HUD
            UIManager.Instance.HUD.SetActive(false);
            
            // Remove pause
            GameManager.Instance.TogglePause();
            
            
            // Change music
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicMenu, false);
            
            LevelManager.Instance.LoadScene("main");
        }
    }
}
