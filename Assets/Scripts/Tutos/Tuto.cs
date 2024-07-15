using System.Collections;
using UnityEngine;
using burglar.managers;

namespace burglar.tutos
{
    public class Tuto : MonoBehaviour
    {
        [SerializeField] GameObject _spawnPoint;
        [SerializeField] private Tuto _nextTuto;

        public TextAsset inkFileStart;
        public TextAsset inkFileEnd;
        private IEnumerator _startDialogEnumerator;
        private IEnumerator _successSequenceEnumerator;

        private void Start()
        {
            _successSequenceEnumerator = SuccessSequence();
            _startDialogEnumerator = DialogManager.Instance.StartDialog();
        }

        public GameObject GetSpawnPoint() { return _spawnPoint; }

        // Optionnel : obtenir le prochain tuto
        public Tuto GetNextTuto()
        {
            return _nextTuto;
        }

        protected void Success()
        {
            Debug.Log("STARTING SUCCESS SEQUENCE");
            
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundSuccess);
            
            StartCoroutine(_successSequenceEnumerator);
        }

        private IEnumerator SuccessSequence()
        {
            Debug.Log("APPEL ON EXIT DE L'ANCIENNE ZONE !!!");
            TutoManager.Instance.GetCurrentTuto().OnExit();
            
            var nextTuto = GetNextTuto();
            
            // Launch first dialog
            yield return StartCoroutine(StartDialogAndWait(TutoManager.Instance.EndTuto()));

            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundTeleport);
            
            // Load next tuto
            TutoManager.Instance.LoadTuto(nextTuto);
            
            // Teleport the player
            yield return StartCoroutine(TeleportPlayerToNewPosition());
            
            Debug.Log("APPEL ON ENTER DE LA NOUVELLE ZONE !!!");
            nextTuto.OnEnter();
            
            // TutoManager.Instance._previousTuto.gameObject.SetActive(false);
            
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundTeleportOut);
            
            // Wait 1s
            yield return new WaitForSeconds(1f);
            
            // Launch second dialog
            StartCoroutine(_startDialogEnumerator);
        }

        private static IEnumerator TeleportPlayerToNewPosition()
        {
            var facingTop = Quaternion.Euler(90, 0, 0);
            // Instantiate TutoManager.Instance.teleportEffectPrefab at player position and rotation to have Y up and position on the ground
            var effect = Instantiate(
                TutoManager.Instance.teleportEffectPrefab, 
                TutoManager.Instance._player.transform.position, 
                facingTop
            );
            
            // Wait 1s
            yield return new WaitForSeconds(1f);
            
            // Hide player and move him to the new position
            TutoManager.Instance._player.gameObject.SetActive(false);
            TutoManager.Instance._player.transform.position = TutoManager.Instance.GetCurrentTuto()._spawnPoint.transform.position;
            
            // Destroy first teleport fx
            Destroy(effect);
            
            // Instantiate second teleport fx
            effect = Instantiate(TutoManager.Instance.teleportEffectPrefab, TutoManager.Instance._player.transform.position, facingTop);
            
            // Wait 1s
            yield return new WaitForSeconds(1f);
            
            // Show player
            TutoManager.Instance._player.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(1f);
            
            // Destroy second teleport fx after 1s
            Destroy(effect);
        }

        public IEnumerator StartDialogAndWait(IEnumerator dialogCoroutine)
        {
            StartCoroutine(dialogCoroutine);

            // Wait until the dialogue is finished
            while (DialogManager.Instance.isInDialog)
            {
                yield return null;
            }
            
            Debug.Log("DIALOG IS OVER");
        }

        public virtual void OnEnter()
        {
            Debug.Log("Tuto started");
        }
        
        public virtual void OnExit()
        {
            Debug.Log("Tuto ended");
        }
    }
}
