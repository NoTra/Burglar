using UnityEngine;
using burglar.managers;

namespace burglar.environment
{
    public class ExitTrigger : MonoBehaviour
    {

        void Start()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Load ShopLevel
                LevelManager.Instance.LoadShop();
            }
        }
    }
}
