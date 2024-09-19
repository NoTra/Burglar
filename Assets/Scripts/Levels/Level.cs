using System.Collections;
using burglar.environment;
using UnityEngine;
using burglar.managers;
using EasyTransition;
using Ink.Runtime;
using System.Collections.Generic;
using burglar.objectives;
using Cinemachine;

namespace burglar.levels
{
    public class Level : MonoBehaviour
    {
        public GameObject _spawnPoint;

        public TextAsset inkFileStart;
        public TextAsset inkFileEnd;
        private IEnumerator _startDialogEnumerator;
        
        [SerializeField] private AgentManager _agentManager;
        
        private Coroutine playerCaughtCoroutine;
        
        [SerializeField] protected List<Credit> _credits;
        [SerializeField] protected List<Safe> _safes;
        [SerializeField] protected List<LightBehavior> _lights;

        [SerializeField] private Camera levelMainCamera;
        
        public int levelIndex;
        
        protected void Start()
        {
            _startDialogEnumerator = DialogManager.Instance.StartDialog();
            
            // Check if LevelManager.Instance._currentLevel match this level
            if (LevelManager.Instance._currentLevelIndex != levelIndex)
            {
                LevelManager.Instance.LoadLevelByIndex(levelIndex);
            }
        }

        public GameObject GetSpawnPoint() { return _spawnPoint; }

        protected void Success()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundSuccess);
        }

        protected void OnPlayerCaught(GameObject agent)
        {
            if (playerCaughtCoroutine != null && GameManager.Instance.gameState == GameManager.GameState.Caught)
            {
                return;
            }
            
            playerCaughtCoroutine = StartCoroutine(PlayerCaughtCoroutine(agent));
        }

        private IEnumerator PlayerCaughtCoroutine(GameObject agent)
        {
            GameManager.Instance.player._playerInput.DeactivateInput();
            
            // Rotate player to face the agent
            GameManager.Instance.player.transform.LookAt(agent.transform);
            
            // Switch camera to Bottom view
            GameManager.Instance.SwitchCameraToBottomRig();

            Debug.Log("Player launch animation");
            GameManager.Instance.player.PlayerAnimator.SetTrigger(Animator.StringToHash("Caught"));
            yield return new WaitForSeconds(2f);
            
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundCaught);
            
            // Dialog Caught
            var story = new Story(DialogManager.Instance.inkFileCaughtAgent.text);
            DialogManager.Instance.SetStory(story);
            yield return StartCoroutine(DialogManager.Instance.StartDialogAndWait());
            
            Debug.Log("Player caught coroutine 01");
            
            TransitionManager.Instance().onTransitionCutPointReached += ResetLevel;
            
            // Launch transition, teleport and reset level
            TransitionManager.Instance().Transition(UIManager.Instance.caughtTransition, 0f);
            
            GameManager.Instance.SwitchCameraToTopRig();
            
            Debug.Log("Player caught coroutine 02");
            yield return new WaitForSecondsRealtime(2f);
            
            GameManager.Instance.player._playerInput.ActivateInput();
            
            playerCaughtCoroutine = null;
        }

        private void ResetLevel()
        {
            // All agents reset
            _agentManager.ResetAgents();
            
            // Reset all credits
            CreditManager.Instance.ResetLevelCredits();
            
            _credits.ForEach(credit =>
            {
                credit.gameObject.SetActive(false);
                credit.gameObject.SetActive(true);
            });
            _safes.ForEach(safe => {
                safe.Reset();
            });
            
            _lights.ForEach(lightBehavior => {
                lightBehavior.ResetLightState();
            });
            
            // Browse all LightSwitch and reset them
            var lightSwitches = FindObjectsOfType<LightSwitch>();
            foreach (var lightSwitch in lightSwitches)
            {
                lightSwitch.Reset();
            }
            
            // Reset player
            ResetPlayer();
            
            TransitionManager.Instance().onTransitionCutPointReached -= ResetLevel;
            
            GameManager.Instance.SetGameState(GameManager.GameState.Playing);
        }

        private void ResetPlayer()
        {
            GameManager.Instance.player.gameObject.SetActive(false);
            GameManager.Instance.player.transform.position = GetSpawnPoint().transform.position;
            GameManager.Instance.player.transform.rotation = Quaternion.identity;
            GameManager.Instance.player.gameObject.SetActive(true);
        }

        public virtual void OnEnter() {}
        public virtual void OnExit() {}
    }
}
