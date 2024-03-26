using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
                EventManager.OnSoundGenerated(hit.point, 0.1f, true);
            }
        }
    }
}
