using System.Collections.Generic;
using UnityEngine;
using burglar.agent;
using burglar.environment;

namespace burglar.tutos
{
    public class Tuto03 : Tuto
    {
        [SerializeField] private List<Safe> safesToCrack;
        [SerializeField] private List<Safe> safesCracked;

        [SerializeField] public TextAsset inkFileSafeFail;
        [SerializeField] private Agent _agent;

        public new void OnEnter()
        {
            _agent.gameObject.SetActive(true);
        }
        
        public new void OnExit()
        {
            _agent.gameObject.SetActive(false);
        }

        private new void Success()
        {
            // Do special success from Tuto02
            base.Success();
        }
    }
}
