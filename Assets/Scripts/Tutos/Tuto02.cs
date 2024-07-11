using System;
using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using burglar.environment;

namespace burglar.tutos
{
    public class Tuto02 : Tuto
    {
        [SerializeField] private List<Safe> safesToCrack;
        private List<Safe> _safesCracked;

        [SerializeField] public TextAsset inkFileSafeFail;

        private void Awake()
        {
            _safesCracked = new List<Safe>();
        }

        private void OnEnable()
        {
            EventManager.SuccessSafeCrack += OnSuccessSafeCrack;
            EventManager.FailSafeCrack += OnFailSafeCrack;
        }

        private void OnDisable()
        {
            EventManager.SuccessSafeCrack -= OnSuccessSafeCrack;
            EventManager.FailSafeCrack -= OnFailSafeCrack;
        }

        private void OnSuccessSafeCrack(Safe safe)
        {
            Debug.Log("Safe crack success");
            if (safesToCrack.Contains(safe))
            {
                Debug.Log("Add safe to safesCracked");
                _safesCracked.Add(safe);
            }
            else
            {
                Debug.Log("Safe not in safesToCrack");
            }

            if (safesToCrack.Count == _safesCracked.Count)
            {
                Success();
            }
            else
            {
                Debug.Log("Not all safes cracked yet");
            }
        }

        private void OnFailSafeCrack(Safe safe)
        {
            Debug.Log("Safe crack fail");
            TutoManager.Instance.SetStory(inkFileSafeFail);
            // Launch special dialog
            StartCoroutine(DialogManager.Instance.StartDialog());
        }

        private new void Success()
        {
            Debug.Log("Success of Tuto02");
            // Do special success from Tuto02
            base.Success();
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Enter Tuto02");
        }
        
        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Exit Tuto02");
        }
    }
}
