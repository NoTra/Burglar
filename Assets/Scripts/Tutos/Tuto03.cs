using UnityEngine;
using burglar.agent;
using burglar.managers;
using burglar.persistence;
using Ink.Runtime;
using System.Collections;
using Cinemachine;
using EasyTransition;

namespace burglar.tutos
{
    public class Tuto03 : Tuto
    {
        [Header("Dialogs")]
        [SerializeField] private TextAsset inkFileWarnAlarm;
        [SerializeField] private TextAsset inkFileCaughtAlarm;
        [SerializeField] private TextAsset inkFileCaughtAgent;
        [SerializeField] private TextAsset inkFileWaypoint01;
        [SerializeField] private TextAsset inkFileWaypoint02;

        // Check if dialog has already been launched
        private bool _boolWarnAlarm = false;
        private Coroutine playerCaughtCoroutine = null;
        
        [Header("Agent")]
        [SerializeField] private Agent _agent;
        private Vector3 _agentStartPosition;
        
        [Header("UserWaypoints")]
        [SerializeField] private UserWaypoint _userWaypoint01;
        [SerializeField] private UserWaypoint _userWaypoint02;

        private void OnEnable()
        {
            EventManager.PlayerCaught += OnPlayerCaught;
            // EventManager.ChangeGameState += OnChangeGameState;
            EventManager.LightChange += OnLightChange;
            EventManager.EnterUserWaypoint += OnEnterUserWaypoint;
            // EventManager.ExitUserWaypoint += OnExitUserWaypoint;
            EventManager.CheckpointReached += OnCheckpointReached;
        }

        private void OnDisable()
        {
            EventManager.PlayerCaught -= OnPlayerCaught;
            // EventManager.ChangeGameState -= OnChangeGameState;
            EventManager.LightChange -= OnLightChange;
            EventManager.EnterUserWaypoint -= OnEnterUserWaypoint;
            EventManager.CheckpointReached -= OnCheckpointReached;
        }

        private void OnCheckpointReached(Checkpoint checkpoint)
        {
            if (checkpoint.gameObject.name != "Checkpoint02_dialog_lasers_win") return;
            
            _boolWarnAlarm = true;
        }

        public override void OnEnter()
        {
            _agent.gameObject.SetActive(true);
            
            // Storing agent start position
            _agentStartPosition = _agent.transform.position;
        }

        public override void OnExit()
        {
            _agent.gameObject.SetActive(false);
        }
        
        private void OnEnterUserWaypoint(UserWaypoint userWaypoint)
        {
            if (
                userWaypoint != _userWaypoint01 && 
                userWaypoint != _userWaypoint02
            ) return;


            if (userWaypoint == _userWaypoint01)
            {
                var story = new Story(inkFileWaypoint01.text);
            
                var dialogManager = DialogManager.Instance;
                dialogManager.SetStory(story);
                
                // Start the dialog
                StartCoroutine(dialogManager.StartDialog());
                
                // Make effect disappear
                _userWaypoint01.gameObject.SetActive(false);
                
                // Make user waypoint 02 appear
                _userWaypoint02.gameObject.SetActive(true);
            }
            else
            {
                var story = new Story(inkFileWaypoint02.text);
                
                var dialogManager = DialogManager.Instance;
                dialogManager.SetStory(story);

                // Stop dialog from running if player uses light switch
                _boolWarnAlarm = true;
                
                // Start the dialog
                StartCoroutine(StartDialogAndGoToNewGame());
            }
        }

        private new void Success()
        {
            // Do special success from Tuto02
            base.Success();
        }

        private void OnPlayerCaught(GameObject player)
        {
            if (playerCaughtCoroutine != null) return;

            playerCaughtCoroutine = StartCoroutine(PlayerCaughtCoroutine());
        }

        private IEnumerator PlayerCaughtCoroutine()
        {
            TutoManager.Instance.SetStory(inkFileCaughtAgent);
            
            _userWaypoint02.gameObject.SetActive(false);
            _userWaypoint01.gameObject.SetActive(true);

            yield return StartCoroutine(DialogManager.Instance.StartDialogAndWait());
            
            TransitionManager.Instance().Transition(UIManager.Instance.caughtTransition, 0f);

            yield return StartCoroutine(TeleportPlayerToNewPosition(GetSpawnPoint().transform.position));
            
            _agent.transform.position = _agentStartPosition;
            _agent.ChangeState(Agent.State.Patrol);
            _agent.GetPatrol().ResetSuspiciousPoint();
            
            GameManager.Instance.SetGameState(GameManager.GameState.Playing);
            
            playerCaughtCoroutine = null;
        }

        // private void OnChangeGameState(GameManager.GameState gameState)
        // {
        //     if (gameState != GameManager.GameState.Alert) return;
        //     
        //     var story = new Story(inkFileCaughtAlarm.text);
        //     
        //     var dialogManager = DialogManager.Instance;
        //     dialogManager.SetStory(story);
        //     StartCoroutine(StartDialogAndRestart());
        // }

        private void OnLightChange(GameObject arg0)
        {
            if (_boolWarnAlarm) return;
            
            var story = new Story(inkFileWarnAlarm.text);
            
            var dialogManager = DialogManager.Instance;
            dialogManager.SetStory(story);
            StartCoroutine(dialogManager.StartDialog());
            
            _boolWarnAlarm = true;
        }
        
        private IEnumerator StartDialogAndGoToNewGame() {
            var dialogManager = DialogManager.Instance;
            yield return StartCoroutine(dialogManager.StartDialogAndWait());

            SaveLoadSystem.Instance.NewGame();
            
            LevelManager.Instance.LoadScene("level1");
        }
    }
}
