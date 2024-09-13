using System;
using System.Collections.Generic;
using burglar.managers;
using UnityEngine;

namespace burglar.objectives
{
    public abstract class ObjectiveManager : MonoBehaviour
    {
        protected Dictionary<int, Dictionary<string, Func<bool>>> Objectives = new ();

        private void OnEnable()
        {
            EventManager.PlayerCaught += ResetObjectives;
        }
        
        private void OnDisable()
        {
            EventManager.PlayerCaught -= ResetObjectives;
        }

        protected void Start()
        {
            Objectives.Clear();
            ClearDisplayedUIObjectives();
        }

        /// <summary>
        /// Display objective on the UI
        /// </summary>
        protected void DisplayUIObjective()
        {
            var uiManager = UIManager.Instance;
            
            // Remove objectives from UI
            for (var i = 0 ; i < uiManager.objectivesContainer.transform.childCount; i++)
            {
                Destroy(uiManager.objectivesContainer.transform.GetChild(i).gameObject);
            }
            
            foreach (var objective in Objectives)
            {
                var objectiveIndex = objective.Key;
                foreach (var obj in objective.Value)
                {
                    // Text = first element of the dictionary object.value
                    var objectiveText = obj.Key;
                    var objectiveValue = obj.Value.Invoke();
                    var newObjective = Instantiate(
                        uiManager.objectivePrefab, 
                        uiManager.objectivesContainer.transform
                    );
                
                    newObjective.GetComponent<Objective>().SetObjective(objectiveIndex, objectiveText, objectiveValue);
                }
            }
        }
        
        
        protected void UpdateUIObjectives()
        {
            Debug.Log("Update Objectives of UI");
            // Crawl every child of the objectives container and update the objective toggle
            foreach (Transform child in UIManager.Instance.objectivesContainer.transform)
            {
                var objective = child.GetComponent<Objective>();
                
                objective.UpdateObjective(Objectives[objective._objectiveIndex][objective._objectiveText]);
            }
        }
        
        private void ClearDisplayedUIObjectives()
        {
            Debug.Log("Clearing displayed objectives from UI");
            foreach (Transform child in UIManager.Instance.objectivesContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Reset objectives and update the UI
        /// </summary>
        /// <param name="arg0"></param>
        public void ResetObjectives(GameObject arg0)
        {
            Debug.Log("Resetting objectives and updating UI");
            // foreach (var objective in Objectives)
            // {
            //     foreach (var obj in objective.Value)
            //     {
            //         obj.Value.Invoke();
            //     }
            // }
            
            UpdateUIObjectives();
        }

        protected Dictionary<int, Dictionary<string, Func<bool>>> GetObjectives()
        {
            return Objectives;
        }
        
        protected abstract void InitObjectives();
    }
}
