using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using burglar.managers;

namespace burglar.objectives
{
    public class ObjectivesLvl2 : ObjectiveManager
    {
        private new void Start()
        {
            base.Start();

            // Clear objectives
            InitObjectives();
        }

        protected override void InitObjectives()
        {
            if (Objectives.Count > 0)
            {
                return;
            }

            if (Objectives == null)
            {
                Objectives = new Dictionary<int, Dictionary<string, Func<bool>>>();
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
            
            Objectives.Add(0, new Dictionary<string, Func<bool>>
            {
                {
                    "Steal the minimum amount to be able to get out. (" + LevelManager.Instance._currentLevel.minimumCredits + ")", 
                    () => 
                        CreditManager.Instance.levelCredit >= 
                          LevelManager.Instance._currentLevel.minimumCredits
                },
                {
                    "[Optional] Steal the maximum amount to get the maximum reward.", 
                    () => 
                        CreditManager.Instance.levelCredit >= 
                        CreditManager.Instance.maximumCredits
                }
            });
            
            DisplayUIObjective();
            UIManager.Instance.UIObjective.SetActive(true);
        }

        private void OnEnable()
        {
            EventManager.UpdateObjectives += UpdateUIObjectives;
            EventManager.PlayerCaught += OnPlayerCaught;
            EventManager.CreditCollected += OnCreditCollected;
        }

        private void OnDisable()
        {
            EventManager.UpdateObjectives -= UpdateUIObjectives;
            EventManager.PlayerCaught -= OnPlayerCaught;
            EventManager.CreditCollected -= OnCreditCollected;
        }

        private void OnCreditCollected(int arg0)
        {
            Debug.Log("Credit Collected from objectives lvl2");
            UpdateUIObjectives();
        }

        private void OnUpdateObjectives()
        {
            Debug.Log("Update Objectives from objectives lvl2");
            UpdateUIObjectives();
        }

        private void OnPlayerCaught(GameObject arg0)
        {
            Debug.Log("OnPlayerCaught from ObjectivesLvl2");
            UpdateUIObjectives();
        }
    }
}
