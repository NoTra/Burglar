using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace burglar
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
