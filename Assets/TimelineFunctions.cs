using UnityEngine;
using burglar.managers;

namespace burglar
{
    public class TimelineFunctions : MonoBehaviour
    {
        public void DeactivatePlayerInput()
        {
            Debug.Log("DeactivatePlayerInput");
            GameManager.Instance.playerInput.DeactivateInput();
        }
        
        public void ActivatePlayerInput()
        {
            Debug.Log("ActivatePlayerInput");
            GameManager.Instance.playerInput.ActivateInput();
        }
    }
}
