using System.Collections;
using UnityEngine;
using burglar.managers;

namespace burglar.tutos
{
    public class Tuto : MonoBehaviour
    {
        [SerializeField] GameObject _spawnPoint;
        [SerializeField] private Tuto _nextTuto;
        [SerializeField] private string _name;

        public TextAsset inkFileStart;
        public TextAsset inkFileEnd;

        public GameObject GetSpawnPoint() { return _spawnPoint; }

        // Optionnel : obtenir le prochain tuto
        public Tuto GetNextTuto()
        {
            return _nextTuto;
        }

        public void Success()
        {
            StartCoroutine(SuccessSequence());
        }

        IEnumerator SuccessSequence()
        {
            // Launch ending dialog
            yield return StartCoroutine(TutoManager.Instance.EndTuto());
        }
    }
}
