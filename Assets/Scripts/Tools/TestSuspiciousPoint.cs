using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static burglar.EventManager;

namespace burglar
{
    public class TestSuspiciousPoint : MonoBehaviour
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Hit: " + hit.point);

                    // Trigger OnSuspiciousPoint event from EventManager instance
                    EventManager.OnSuspectedPoint(hit.point);
                }
            }
        }
    }
}
