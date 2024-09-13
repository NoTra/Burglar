using UnityEngine;
using burglar.managers;

namespace burglar.levels
{
    public class Level01 : Level
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
            
            // Set the minimum credits
            CreditManager.Instance.minimumCredits = LevelManager.Instance._currentLevel.minimumCredits;
            
            CreditManager.Instance.SetMaxCredits(maxCredits);
            
            Debug.Log("Current level ID : " + LevelManager.Instance._currentLevelIndex + " (" + LevelManager.Instance._currentLevel.levelName + ")");
            Debug.Log("Current level minimum credit requirement : " + LevelManager.Instance._currentLevel.minimumCredits);
        }

        private new void OnPlayerCaught(GameObject arg0)
        {
            Debug.Log("Player Caught from Level01");
            base.OnPlayerCaught(arg0);
        }

        private new void Success()
        {
            base.Success();
        }
    }
}
