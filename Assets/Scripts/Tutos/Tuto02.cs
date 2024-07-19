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
            if (safesToCrack.Contains(safe))
            {
                _safesCracked.Add(safe);
            }

            if (safesToCrack.Count == _safesCracked.Count)
            {
                Success();
            }
        }

        private void OnFailSafeCrack(Safe safe)
        {
            TutoManager.Instance.SetStory(inkFileSafeFail);
            // Launch special dialog
            StartCoroutine(DialogManager.Instance.StartDialog());
        }

        private new void Success()
        {
            // Do special success from Tuto02
            base.Success();
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
