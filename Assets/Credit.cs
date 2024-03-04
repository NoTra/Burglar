using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class Credit : MonoBehaviour
    {
        [SerializeField] private int _value;

        public int Value => _value;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.OnCreditCollected(_value);

                // PlaySound

                Destroy(gameObject);
            }
        }

    }
}
