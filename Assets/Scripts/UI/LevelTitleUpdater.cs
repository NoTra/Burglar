using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using TMPro;

namespace burglar
{
    public class LevelTitleUpdater : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.LoadLevelStart += UpdateLevelTitle;
        }
        
        private void OnDisable()
        {
            EventManager.LoadLevelStart -= UpdateLevelTitle;
        }

        // Start is called before the first frame update
        private void UpdateLevelTitle()
        {
            var TMPTitle = GetComponent<TextMeshProUGUI>();
            TMPTitle.text = LevelManager.Instance._currentLevel.levelName;
        }
    }
}
