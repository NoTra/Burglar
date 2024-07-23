using UnityEngine;
using burglar.managers;

namespace burglar.environment
{
    public class ExitTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (LevelManager.Instance._currentLevel.minimumCredits > CreditManager.Instance.levelCredit)
                {
                    Debug.Log("You need more credits to exit the level");
                }
                else
                {
                    // Load ShopLevel
                    LevelManager.Instance.LoadShop();
                }
            }
        }
    }
}
