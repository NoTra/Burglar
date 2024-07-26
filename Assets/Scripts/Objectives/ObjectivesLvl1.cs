using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using burglar.managers;

namespace burglar
{
    public class ObjectivesLvl1 : ObjectiveManager
    {
        private new void Start()
        {
            base.Start();
        }

        protected override void InitObjectives()
        {
            if (_objectives == null)
            {
                _objectives = new Dictionary<int, Dictionary<string, Func<bool>>>();
            }
            
            if (CreditManager.Instance == null)
            {
                Debug.LogError("CreditManager is null");
                return;
            }
            
            if (LevelManager.Instance == null)
            {
                Debug.LogError("LevelManager is null");
                return;
            }
            
            _objectives.Add(0, new Dictionary<string, Func<bool>>
            {
                {
                    "Steal the minimum amount to be able to get out !", 
                    () => 
                        CreditManager.Instance.levelCredit >= 
                          LevelManager.Instance._currentLevel.minimumCredits
                }
            });
        }

        private void OnEnable()
        {
            EventManager.CreditCollected += OnCreditCollected;
            EventManager.ObjectiveLoaded += OnObjectiveLoaded;
        }

        private void OnDisable()
        {
            EventManager.CreditCollected -= OnCreditCollected;
            EventManager.ObjectiveLoaded -= OnObjectiveLoaded;
        }
        
        private void OnObjectiveLoaded()
        {
            InitObjectives();
            
            DisplayObjective();
        }
        
        private void OnCreditCollected(int amount)
        {
            UpdateObjective();
        }
    }
}
