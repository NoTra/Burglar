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
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _playerMoving || DialogManager.Instance.isInDialog) return;
            Debug.Log("Current level : " + LevelManager.Instance._currentLevelIndex);
            // Player has not collected enough credits
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
                EventManager.OnLevelSuccess();
                
                // If current level is the last level
                if (LevelManager.Instance._currentLevelIndex == LevelManager.Instance.levels.Count - 1)
                {
                    // Load EndGame
                    // LevelManager.Instance.LoadEndGame();
                    LevelManager.Instance.LoadEndCredits();
                }
                else
                {
                    // Load ShopLevel
                    LevelManager.Instance.LoadShop();
                }
            }
        }

        private IEnumerator StartDialogAndForcePlayerToWalkTo(NavMeshAgent playerNavmesh, Vector3 position)
        {
            while (playerNavmesh.pathPending)
            {
                yield return null;
            }
            
            // Deactivate player collision
            // var playerCapsuleCollider = GameManager.Instance.player.GetComponent<CapsuleCollider>();
            // playerCapsuleCollider.enabled = false;
            
            playerNavmesh.SetDestination(position);
            GameManager.Instance.player._playerInput.DeactivateInput();
            
            GameManager.Instance.player.PlayerAnimator.SetBool(IsWalking, true);
            _playerMoving = true;
            yield return new WaitUntil(() =>
            {
                return playerNavmesh.remainingDistance < playerNavmesh.stoppingDistance;
            });
            _playerMoving = false;
            GameManager.Instance.player.PlayerAnimator.SetBool(IsWalking, false);
            GameManager.Instance.player._playerInput.ActivateInput();
            
            playerNavmesh.ResetPath();
            
            var dialogManager = DialogManager.Instance;
            yield return StartCoroutine(dialogManager.StartDialogAndWait());
            
            _dialogCoroutine = null;
        }
    }
}
