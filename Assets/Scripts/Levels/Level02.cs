using UnityEngine;
using burglar.managers;

namespace burglar.levels
{
    public class Level02 : Level
    {
        private void OnEnable()
        {
            EventManager.PlayerCaught += OnPlayerCaught;
        }

        private void OnDisable()
        {
            EventManager.PlayerCaught -= OnPlayerCaught;
        }
        
        private new void Start()
        {
            base.Start();

            var maxCredits = 0;
            
            // Send the credits to CreditManager
            foreach (var credit in base._credits)
            {
                maxCredits += credit.Value;
            }
            
            // Safes
            foreach (var safe in base._safes)
            {
                maxCredits += safe.Value;
            }
            
            CreditManager.Instance.SetMaxCredits(maxCredits);
        }

        private new void OnPlayerCaught(GameObject arg0)
        {
            base.OnPlayerCaught(arg0);
        }

        private new void Success()
        {
            base.Success();
        }
    }
}
