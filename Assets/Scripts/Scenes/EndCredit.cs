using System;
using System.Collections;
using System.Collections.Generic;
using burglar.managers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace burglar
{
    public class EndCredit : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;

        private void Start()
        {
            StartCoroutine(EndingCoroutine());
        }

        private IEnumerator EndingCoroutine()
        {
            playableDirector?.Play();
            
            yield return new WaitForSeconds((float)playableDirector.duration);
            
            SwitchToMainMenu();
        }

        private void SwitchToMainMenu()
        {
            Debug.Log("CinematicEnd launch main scene");
            LevelManager.Instance.LoadMainMenu();
        }
    }
}
