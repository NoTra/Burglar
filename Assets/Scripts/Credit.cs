using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace burglar
{
    public class Credit : Interactible
    {
        [SerializeField] private int _value;

        public int Value => _value;

        protected override void Interact()
        {
            EventManager.OnCreditCollected(_value);

            // PlaySound

            Destroy(gameObject);
        }
    }
}
