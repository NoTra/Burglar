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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundSuccess);
            
            StartCoroutine(_successSequenceEnumerator);
        }

        private IEnumerator SuccessSequence()
        {
            TutoManager.Instance.GetCurrentTuto().OnExit();
            
            var previousTuto = TutoManager.Instance.GetCurrentTuto();
            var nextTuto = GetNextTuto();
            
            // Launch first dialog
            TutoManager.Instance.UnloadTuto();
            yield return StartCoroutine(DialogManager.Instance.StartDialogAndWait());

            // Load next tuto
            TutoManager.Instance.LoadTuto(nextTuto);
            
            // Teleport the player
            var newPosition = TutoManager.Instance.GetCurrentTuto()._spawnPoint.transform.position;
            yield return StartCoroutine(TeleportPlayerToNewPosition(newPosition));
            
            nextTuto.OnEnter();
            
            // TutoManager.Instance._previousTuto.gameObject.SetActive(false);
            
            // Launch second dialog
            yield return StartCoroutine(DialogManager.Instance.StartDialogAndWait());
            
            previousTuto.gameObject.SetActive(false);
        }

        public static IEnumerator TeleportPlayerToNewPosition(Vector3 newPosition)
        {
            var facingTop = Quaternion.Euler(90, 0, 0);
            // Instantiate TutoManager.Instance.teleportEffectPrefab at player position and rotation to have Y up and position on the ground
            var effect = Instantiate(
                TutoManager.Instance.teleportEffectPrefab, 
                TutoManager.Instance._player.transform.position, 
                facingTop
            );
            // Bump up the scale of the teleport effect
            effect.transform.localScale = new Vector3(2f, 2f, 2f);
            
            // Make player disappear
            TutoManager.Instance._player.gameObject.SetActive(false);
            
            var tpSound = AudioManager.Instance.PlaySFX(AudioManager.Instance.soundTeleport);
            
            // Wait 0.5s
            yield return new WaitForSeconds(0.5f);
            
            tpSound.Stop();
            
            // Wait 1s
            yield return new WaitForSeconds(0.5f);
            
            // Move him to the new position
            TutoManager.Instance._player.transform.position = newPosition;
            
            // Destroy first teleport fx
            Destroy(effect);
            
            // Move camera to player's position
            TutoManager.Instance._freeLook.Follow = TutoManager.Instance._player.transform;
            
            yield return new WaitForSeconds(0.5f);
            
            // Instantiate second teleport fx
            effect = Instantiate(TutoManager.Instance.teleportEffectPrefab, TutoManager.Instance._player.transform.position, facingTop);
            // Bump up the scale of the teleport effect
            effect.transform.localScale = new Vector3(2f, 2f, 2f);
            
            tpSound = AudioManager.Instance.PlaySFX(AudioManager.Instance.soundTeleportOut);
            
            // Show player
            TutoManager.Instance._player.gameObject.SetActive(true);
            
            // Wait 0.5s
            yield return new WaitForSeconds(0.5f);
            
            tpSound.Stop();
            
            // Destroy second teleport fx after 1s
            Destroy(effect);
        }

        public virtual void OnEnter()
        {
            
        }
        
        public virtual void OnExit()
        {
            
        }
    }
}
