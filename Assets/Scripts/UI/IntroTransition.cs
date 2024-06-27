using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using burglar.environment;
using burglar.tutos;

namespace burglar.UI
{
    public class IntroTransition : MonoBehaviour
    {
        [SerializeField] private List<Interactible> _interactibleSuccess;
        [SerializeField] private Tuto _nextTuto;
        [SerializeField] private GameObject _playerSpawnPoint;
        [SerializeField] private burglar.player.Player _player;

        private void OnEnable()
        {
            EventManager.Interact += (interactible) => OnInteract(interactible);
        }

        private void OnInteract(Interactible interactible)
        {
            // si interactible in _interactibleSuccess
            if (_interactibleSuccess.Contains(interactible))
            {
                _interactibleSuccess.Remove(interactible);
            }

            if (_interactibleSuccess.Count <= 0)
            {
                // Success
                _player.transform.position = _playerSpawnPoint.transform.position;

            }
        }
    }
}
