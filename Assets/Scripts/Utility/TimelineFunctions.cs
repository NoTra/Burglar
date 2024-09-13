using UnityEngine;
using burglar.managers;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace burglar.utility
{
    public class TimelineFunctions : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Image _radial;

        public void DeactivatePlayerInput()
        {
            GameManager.Instance.player._playerInput.DeactivateInput();
        }

        public void ActivatePlayerInput()
        {
            GameManager.Instance.player._playerInput.ActivateInput();
        }

        public void StartTuto()
        {
            TutoManager.Instance.StartTuto();
        }
        
        public void HideHUD()
        {
            UIManager.Instance.HideHud();
        }
        
        public void ToggleHUD()
        {
            UIManager.Instance.ToggleHudVisibility();
        }
    }
}
