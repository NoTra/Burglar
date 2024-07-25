using System;
using System.Collections.Generic;
using UnityEngine;

using burglar.environment;
using burglar.managers;

namespace burglar
{
    public class ObjectivesTutorial : ObjectiveManager
    {
        [Header("1st room")]
        private bool _objectTaken = false;
        [SerializeField] private GameObject _objectToTake;
        
        [Header("2nd room")]
        private bool _safeSuccessFullyOpened = false;
        [SerializeField] private GameObject _safe;
        
        [Header("3rd room")]
        private bool _hasSwitchOnLight = false;
        [SerializeField] private List<LightSwitch> _lightSwitches;
        
        private bool _hasPassedUserWaypoint1 = false;
        [SerializeField] private UserWaypoint _userWaypoint1;
        
        private bool _hasPassedUserWaypoint2 = false;
        [SerializeField] private UserWaypoint _userWaypoint2;
        
        private new void Start()
        {
            base.Start();

            InitObjectives();
            
            DisplayObjective();
        }

        protected override void InitObjectives()
        {
            if (_objectives == null)
            {
                _objectives = new Dictionary<int, Dictionary<string, Func<bool>>>();
            }
            
            _objectives.Add(0, new Dictionary<string, Func<bool>>
            {
                {
                    "Take the object on the table (press E)",
                    () => _objectTaken
                },
                {
                    "Open the safe (press E)",
                    () => _safeSuccessFullyOpened
                },
                {
                    "(Optional) Turn on the light switch (press E)",
                    () => _hasSwitchOnLight
                },
                {
                    "Go to the first user waypoint",
                    () => _hasPassedUserWaypoint1
                },
                {
                    "Go to the second user waypoint",
                    () => _hasPassedUserWaypoint2
                }
            });
        }
        
        private void OnEnable()
        {
            EventManager.Interact += OnInteract;
            EventManager.SuccessSafeCrack += OnSuccessSafeCrack;
            EventManager.LightChange += OnLightChange;
            EventManager.EnterUserWaypoint += OnEnterUserWaypoint;
        }

        private void OnDisable()
        {
            EventManager.Interact -= OnInteract;
            EventManager.SuccessSafeCrack -= OnSuccessSafeCrack;
            EventManager.LightChange -= OnLightChange;
            EventManager.EnterUserWaypoint -= OnEnterUserWaypoint;
        }
        
        private void OnInteract(Interactible interactible)
        {
            if (!_objectTaken && interactible.gameObject == _objectToTake)
            {
                _objectTaken = true;
                UpdateObjective();
            }
        }
        
        private void OnSuccessSafeCrack(Safe safe)
        {
            if (!_safeSuccessFullyOpened && safe.gameObject == _safe)
            {
                _safeSuccessFullyOpened = true;
                UpdateObjective();
            }
        }
        
        private void OnEnterUserWaypoint(UserWaypoint userWaypoint)
        {
            if (!_hasPassedUserWaypoint1 && userWaypoint == _userWaypoint1)
            {
                _hasPassedUserWaypoint1 = true;
                UpdateObjective();
            }
            else if (!_hasPassedUserWaypoint2 && userWaypoint == _userWaypoint2)
            {
                _hasPassedUserWaypoint2 = true;
                UpdateObjective();
            }
        }

        private void OnLightChange(GameObject light)
        {
            if (!_hasSwitchOnLight && _lightSwitches.Contains(light.GetComponent<LightSwitch>()))
            {
                _hasSwitchOnLight = true;
                UpdateObjective();
            }
        }
    }
}
