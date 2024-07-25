using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using TMPro;

namespace burglar
{
    public class LevelTitleUpdater : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            var TMPTitle = GetComponent<TextMeshProUGUI>();
            TMPTitle.text = LevelManager.Instance._currentLevel.levelName;
        }
    }
}
