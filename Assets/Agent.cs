using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class Agent : MonoBehaviour
    {
        public enum State
        {
            Patrol,
            Suspicious,
            Search,
            Chase
        }

        private State _currentState;

        public float _earingDistance = 10f;

        private void Start()
        {
            _currentState = State.Patrol;
        }


    }
}
