using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace burglar
{
    public class Waypoints : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _waypoints = new List<GameObject>();
        private int _currentWaypointIndex = 0;
        private NavMeshAgent _agent;
        [SerializeField] private float _waitTimeBetweenPoints;
        private Coroutine _waitCoroutine;

        private Vector3 _surprisePoint;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            var destinationGO = _waypoints[_currentWaypointIndex];
            var destinationPosition = new Vector3(destinationGO.transform.position.x, destinationGO.transform.position.y, destinationGO.transform.position.z);
            _agent.SetDestination(destinationPosition);
        }

        private void OnEnable()
        {
            EventManager.SuspectedPoint += (point) => OnSuspiciousPoint(point);
        }

        private void OnDisable()
        {
            EventManager.SuspectedPoint -= (point) => OnSuspiciousPoint(point);
        }

        private void OnSuspiciousPoint(Vector3 point)
        {
            _surprisePoint = point;
            _agent.SetDestination(point);
        }

        private void Update()
        {
            // We have a suspicious point to go to ?
            if (_surprisePoint != Vector3.zero)
            {
                if (_agent.remainingDistance < 0.5f)
                {
                    _surprisePoint = Vector3.zero;

                    _waitCoroutine = StartCoroutine(WaitBeforeContinueWaypoint());
                }
                return;
            }

            if (_agent.remainingDistance < 0.5f && _waitCoroutine == null)
            {
                _waitCoroutine = StartCoroutine(WaitBeforeNextWaypoint());
            }
        }

        private IEnumerator WaitBeforeNextWaypoint()
        {
            yield return new WaitForSeconds(_waitTimeBetweenPoints);

            GoToNextWaypoint();

            yield return null;
        }

        private IEnumerator WaitBeforeContinueWaypoint()
        {
            yield return new WaitForSeconds(_waitTimeBetweenPoints);

            ResumePatrol();

            yield return null;
        }

        private void GoToNextWaypoint()
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }

            var newDestinationGO = _waypoints[_currentWaypointIndex];
            var newDestinationPosition = new Vector3(newDestinationGO.transform.position.x, newDestinationGO.transform.position.y, newDestinationGO.transform.position.z);
            _agent.SetDestination(newDestinationPosition);

            _waitCoroutine = null;
        }

        private void ResumePatrol()
        {
            _agent.SetDestination(_waypoints[_currentWaypointIndex].transform.position);

            _waitCoroutine = null;
        }
    }
}
