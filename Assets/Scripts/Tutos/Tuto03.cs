using UnityEngine;
using burglar.agent;
using burglar.managers;
using burglar.persistence;
using Ink.Runtime;
using System.Collections;

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
        
        [Header("Agent")]
        [SerializeField] private Agent _agent;
        private Vector3 _agentStartPosition;
        
        [Header("UserWaypoints")]
        [SerializeField] private UserWaypoint _userWaypoint01;
        [SerializeField] private UserWaypoint _userWaypoint02;

        private void OnEnable()
        {
            EventManager.PlayerCaught += OnPlayerCaught;
            EventManager.ChangeGameState += OnChangeGameState;
            EventManager.LightChange += OnLightChange;
            EventManager.EnterUserWaypoint += OnEnterUserWaypoint;
            // EventManager.ExitUserWaypoint += OnExitUserWaypoint;
            EventManager.CheckpointReached += OnCheckpointReached;
        }

        private void OnDisable()
        {
            EventManager.PlayerCaught -= OnPlayerCaught;
            EventManager.ChangeGameState -= OnChangeGameState;
            EventManager.LightChange -= OnLightChange;
            EventManager.EnterUserWaypoint -= OnEnterUserWaypoint;
            EventManager.CheckpointReached -= OnCheckpointReached;
            // EventManager.ExitUserWaypoint -= OnExitUserWaypoint;
        }

        private void OnCheckpointReached(Checkpoint checkpoint)
        {
            if (checkpoint.gameObject.name != "Checkpoint02_dialog_lasers_win") return;
            
            Debug.Log("Checkpoint 02 reached, deactivate switch dialog");
            _boolWarnAlarm = true;
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter Tuto03");
            _agent.gameObject.SetActive(true);
            
            // Storing agent start position
            _agentStartPosition = _agent.transform.position;
        }

        public override void OnExit()
        {
            Debug.Log("OnExit Tuto03");
            _agent.gameObject.SetActive(false);
        }
        
        private void OnEnterUserWaypoint(UserWaypoint userWaypoint)
        {
            if (
                userWaypoint.gameObject.name != _userWaypoint01.gameObject.name && 
                userWaypoint.gameObject.name != _userWaypoint02.gameObject.name
            ) return;


            if (userWaypoint.gameObject.name == _userWaypoint01.gameObject.name)
            {
                Debug.Log("UserWaypoint 01 reached");

                var story = new Story(inkFileWaypoint01.text);
            
                var dialogManager = DialogManager.Instance;
                dialogManager.SetStory(story);
                
                // Start the dialog
                StartCoroutine(dialogManager.StartDialog());
                
                // Make effect disappear
                userWaypoint.gameObject.SetActive(false);
                
                // Make user waypoint 02 appear
                _userWaypoint02.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("UserWaypoint 02 reached");
                var story = new Story(inkFileWaypoint02.text);
                
                var dialogManager = DialogManager.Instance;
                dialogManager.SetStory(story);

                Debug.Log("Set BoolWarnAlarm to false");
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
            Debug.Log("Player caught");
            TutoManager.Instance.SetStory(inkFileCaughtAgent);
            
            _agent.transform.position = _agentStartPosition;
            _agent.ChangeState(Agent.State.Patrol);
            player.transform.position = GetSpawnPoint().transform.position;
            
            StartCoroutine(StartDialogAndRestart());
        }

        private void OnChangeGameState(GameManager.GameState gameState)
        {
            if (gameState != GameManager.GameState.Alert) return;
            
            var story = new Story(inkFileCaughtAlarm.text);
            
            var dialogManager = DialogManager.Instance;
            dialogManager.SetStory(story);
            Debug.Log("Change game state to alert so dialog launch !");
            StartCoroutine(StartDialogAndRestart());
        }
        
        private IEnumerator StartDialogAndRestart()
        {
            yield return StartCoroutine(DialogManager.Instance.StartDialog());
            
            Debug.Log("Dialog is over, now restart tuto");
            TutoManager.Instance._player.transform.position = GetSpawnPoint().transform.position;
        }

        private void OnLightChange(GameObject arg0)
        {
            if (_boolWarnAlarm) return;
            
            Debug.Log("BoolWarnAlarm is false, launch dialog !");
            
            var story = new Story(inkFileWarnAlarm.text);
            
            var dialogManager = DialogManager.Instance;
            dialogManager.SetStory(story);
            Debug.Log("Change game state to alert so dialog launch !");
            StartCoroutine(dialogManager.StartDialog());
            
            _boolWarnAlarm = true;
        }
        
        private IEnumerator StartDialogAndGoToNewGame() {
            var dialogCoroutine = DialogManager.Instance.StartDialog();
            yield return StartCoroutine(StartDialogAndWait(dialogCoroutine));

            // Wait until the dialogue is finished
            // while (DialogManager.Instance.isInDialog)
            // {
            //     yield return null;
            // }
            
            Debug.Log("DIALOG IS OVER");
            
            SaveLoadSystem.Instance.NewGame();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicLevel, false);
        }
    }
}
