using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using burglar.environment;

namespace burglar.tutos
{
    public class Tuto02 : Tuto
    {
        [SerializeField] private List<Safe> safesToCrack;
        [SerializeField] private List<Safe> safesCracked;

        [SerializeField] public TextAsset inkFileSafeFail;

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
                safesToCrack.Add(safe);
            }

            if (safesToCrack.Count == safesCracked.Count)
            {
                Success();
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
            // Do special success from Tuto02
            base.Success();
        }
    }
}
