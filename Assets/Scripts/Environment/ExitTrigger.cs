using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using burglar.managers;
using Ink.Runtime;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace burglar.environment
{
    public class ExitTrigger : MonoBehaviour
    {
        public TextAsset inkFileNoMinCredits;
        private Coroutine _dialogCoroutine;
        
        [SerializeField] private Transform playerStartLocation;
        private bool _playerMoving = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _playerMoving || DialogManager.Instance.isInDialog) return;
            
            if (LevelManager.Instance._currentLevel.minimumCredits > CreditManager.Instance.levelCredit)
            {
                if (_dialogCoroutine != null)
                {
                    return;
                }
                
                var story = new Story(inkFileNoMinCredits.text);
                DialogManager.Instance.SetStory(story);
                
                // TP player to start of the level
                var playerNavmesh = other.gameObject.GetComponent<NavMeshAgent>();
                _dialogCoroutine = StartCoroutine(StartDialogAndForcePlayerToWalkTo(playerNavmesh, playerStartLocation.position));
            }
            else
            {
                // Load ShopLevel
                LevelManager.Instance.LoadShop();
            }
        }

        private IEnumerator StartDialogAndForcePlayerToWalkTo(NavMeshAgent playerNavmesh, Vector3 position)
        {
            while (playerNavmesh.pathPending)
            {
                Debug.Log("Path pending...");
                yield return null;
            }
            
            Debug.Log("Making player walk to " + position);
            // Deactivate player collision
            // var playerCapsuleCollider = GameManager.Instance.player.GetComponent<CapsuleCollider>();
            // playerCapsuleCollider.enabled = false;
            
            playerNavmesh.SetDestination(position);
            Debug.Log("playerNavmesh.remainingDistance: " + playerNavmesh.remainingDistance);
            GameManager.Instance.playerInput.DeactivateInput();
            
            GameManager.Instance.player.PlayerAnimator.SetBool("isWalking", true);
            _playerMoving = true;
            yield return new WaitUntil(() =>
            {
                Debug.Log("Walking to destination... (distance remaining: " + playerNavmesh.remainingDistance + ")"); 
                return playerNavmesh.remainingDistance < playerNavmesh.stoppingDistance;
            });
            _playerMoving = false;
            GameManager.Instance.player.PlayerAnimator.SetBool("isWalking", false);
            GameManager.Instance.playerInput.ActivateInput();
            // playerCapsuleCollider.enabled = true;
            
            playerNavmesh.ResetPath();
            Debug.Log("End (StartDialogAndForcePlayerToWalkTo)");
            
            var dialogManager = DialogManager.Instance;
            yield return StartCoroutine(dialogManager.StartDialogAndWait());
            
            _dialogCoroutine = null;
        }
    }
}
