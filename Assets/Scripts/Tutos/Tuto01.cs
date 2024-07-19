using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using burglar.environment;

namespace burglar.tutos
{
    public class Tuto01 : Tuto
    {
        [SerializeField] private List<Interactible> itemsToPickUp;
        [SerializeField] private List<Interactible> itemsPickedUp;

        private void OnEnable()
        {
            EventManager.Interact += OnInteract;
        }

        private void OnDisable()
        {
            EventManager.Interact -= OnInteract;
        }

        private void OnInteract(Interactible interactible)
        {
            if (itemsToPickUp.Contains(interactible))
            {
                itemsPickedUp.Add(interactible);
            }

            if (itemsPickedUp.Count == itemsToPickUp.Count) {
                Success();
            }
        }

        private new void Success()
        {
            // Do special success from Tuto01
            base.Success();
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
