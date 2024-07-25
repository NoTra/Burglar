using UnityEngine;
using burglar.managers;

namespace burglar
{
    public class TimelineFunctions : MonoBehaviour
    {
        public void DeactivatePlayerInput()
        {
            GameManager.Instance.player._playerInput.DeactivateInput();
        }
        
        public void ActivatePlayerInput()
        {
            GameManager.Instance.player._playerInput.ActivateInput();
        }
    }
}
