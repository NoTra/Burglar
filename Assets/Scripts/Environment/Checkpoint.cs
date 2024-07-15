using System;
using System.Collections;
using System.Collections.Generic;
using burglar.managers;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace burglar
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private TextAsset checkpointDialogAsset;
        [SerializeField] private bool alreadySeen = false;

        private void OnTriggerEnter(Collider other)
        {
            if (alreadySeen) return;
            
            // Load and launch dialog
            var story = new Story(checkpointDialogAsset.text);
            DialogManager.Instance.SetStory(story);
            StartCoroutine(DialogManager.Instance.StartDialog());
            
            alreadySeen = true;
            
            EventManager.OnCheckpointReached(this);
        }
    }
}
