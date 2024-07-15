using System;
using System.Collections;
using System.Collections.Generic;
using burglar.managers;
using UnityEngine;

namespace burglar
{
    public class UserWaypoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Send event OnEnterUserWaypoint");
                EventManager.OnEnterUserWaypoint(this);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Send event OnEnterUserWaypoint");
                EventManager.OnExitUserWaypoint(this);
            }
        }
    }
}
