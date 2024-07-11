using System;
using System.Collections.Generic;
using UnityEngine;
using burglar.agent;
using burglar.environment;
using burglar.managers;

namespace burglar.tutos
{
    public class Tuto03 : Tuto
    {
        [SerializeField] private List<Safe> safesToCrack;
        [SerializeField] private List<Safe> safesCracked;

        [SerializeField] public TextAsset inkFileSafeFail;
        [SerializeField] private Agent _agent;
        private Vector3 _agentStartPosition;

        private void OnEnable()
        {
            EventManager.PlayerCaught += OnPlayerCaught;
        }
        
        private void OnDisable()
        {
            EventManager.PlayerCaught -= OnPlayerCaught;
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
        
        private new void Success()
        {
            // Do special success from Tuto02
            base.Success();
        }
        
        private void OnPlayerCaught(GameObject player)
        {
            Debug.Log("Player caught");
            TutoManager.Instance.SetStory(inkFileSafeFail);
            
            _agent.transform.position = _agentStartPosition;
            player.transform.position = GetSpawnPoint().transform.position;
            
            StartCoroutine(DialogManager.Instance.StartDialog());
        }
    }
}
