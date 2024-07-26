using System;
using System.Collections.Generic;
using burglar.managers;
using UnityEngine;

namespace burglar
{
    public abstract class ObjectiveManager : MonoBehaviour
    {
        protected Dictionary<int, Dictionary<string, Func<bool>>> _objectives = new Dictionary<int, Dictionary<string, Func<bool>>>();
        
        // Start is called before the first frame update
        protected void Start()
        {
            Debug.Log("Starting objectives (ObjectiveManager)");
            UIManager.Instance.UIObjective.SetActive(true);
            ClearDisplayedObjectives();
        }

        protected void DisplayObjective()
        {
            var uiManager = UIManager.Instance;
            foreach (var objective in _objectives)
            {
                var objectiveIndex = objective.Key;
                foreach (var obj in objective.Value)
                {
                    // Debug.Log("Objective: id -> " + objectiveIndex + " text -> " + obj.Key + " value -> " + obj.Value);
                    
                    // Text = first element of the dictionary object.value
                    var objectiveText = obj.Key;
                    var objectiveValue = obj.Value.Invoke();
                    var newObjective = Instantiate(
                        uiManager.objectivePrefab, 
                        uiManager.objectivesContainer.transform
                    );
                
                    // TODO: Make it appear with style
                
                    newObjective.GetComponent<Objective>().SetObjective(objectiveIndex, objectiveText, objectiveValue);
                }
            }
        }
        
        protected void UpdateObjective()
        {
            // Crawl every child of the objectives container and update the objective toggle
            foreach (Transform child in UIManager.Instance.objectivesContainer.transform)
            {
                var objective = child.GetComponent<Objective>();
                
                objective.UpdateObjective(_objectives[objective._objectiveIndex][objective._objectiveText]);
            }
        }
        
        private void ClearDisplayedObjectives()
        {
            foreach (Transform child in UIManager.Instance.objectivesContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        protected Dictionary<int, Dictionary<string, Func<bool>>> GetObjectives()
        {
            return _objectives;
        }
        
        protected abstract void InitObjectives();
    }
}
