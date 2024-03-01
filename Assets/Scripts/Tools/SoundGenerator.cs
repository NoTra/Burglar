using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static burglar.EventManager;

namespace burglar
{
    public class SoundGenerator : MonoBehaviour
    {
        private void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            // On left click, get coordinates and send event
            if (Input.GetMouseButtonDown(0))
            {
                GenerateSound();
            }
        }

        private void GenerateSound()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit: " + hit.point);

                EventManager.OnSoundHeard(hit.point, 0.1f);
            }
        }
    }
}
