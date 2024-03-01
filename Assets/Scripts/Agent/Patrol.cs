using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace burglar
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _waypoints = new List<GameObject>();
        private int _currentWaypointIndex = 0;
        private Agent _agent;
        private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _searchTimeBetweenPoints;
        private Coroutine _searchCoroutine;

        private Vector3 _suspiciousPoint;
        [SerializeField] private GameObject _suspiciousIcon;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _agent = GetComponent<Agent>();
        }

        private void Start()
        {
            var destinationGO = _waypoints[_currentWaypointIndex];
            var destinationPosition = new Vector3(destinationGO.transform.position.x, destinationGO.transform.position.y, destinationGO.transform.position.z);
            _navMeshAgent.SetDestination(destinationPosition);
        }

        private void OnEnable()
        {
            EventManager.SoundHeard += (point, strength) => CheckSuspiciousPoint(point);
        }

        private void OnDisable()
        {
            EventManager.SoundHeard -= (point, strength) => CheckSuspiciousPoint(point);
        }

        private void CheckSuspiciousPoint(Vector3 point)
        {
            Debug.Log("Check distance : " + Vector3.Distance(transform.position, point));
            Debug.Log("Earing distance : " + _agent._earingDistance);
            // Check if the point is in range of agent's earing radius
            if (Vector3.Distance(transform.position, point) > _agent._earingDistance)
            {
                Debug.Log("Sound is too far, no movement");
                return;
            }

            _suspiciousPoint = point;

            var direction = (point - transform.position).normalized * 1.5f;
            var pointToGo = point - direction;

            _navMeshAgent.SetDestination(pointToGo);
        }

        private void Update()
        {
            // We have a suspicious point to go to ?
            if (_suspiciousPoint != Vector3.zero)
            {
                // We stop the searching coroutine if it's running
                if (_searchCoroutine != null)
                {
                    StopCoroutine(_searchCoroutine);
                    _searchCoroutine = null;
                }

                // Display suspicious icon
                _suspiciousIcon.SetActive(true);

                if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
                {
                    _suspiciousPoint = Vector3.zero;

                    _searchCoroutine = StartCoroutine(SearchBeforeContinueWaypoint());
                }
                return;
            }

            if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance && _searchCoroutine == null)
            {
                _searchCoroutine = StartCoroutine(SearchBeforeNextWaypoint());
            }
        }

        private IEnumerator SearchBeforeNextWaypoint()
        {
            // Rotate player to match the rotation of the waypoint
            var waypointRotation = _waypoints[_currentWaypointIndex].transform.rotation;
            var playerStartRotation = transform.rotation;

            float elapsedTime = 0f;
            while (elapsedTime < 1.0f)
            {
                transform.rotation = Quaternion.Slerp(playerStartRotation, waypointRotation, elapsedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = waypointRotation;

            yield return TurnLeftAndRight();

            GoToNextWaypoint();
        }

        private IEnumerator TurnLeftAndRight()
        {
            // Turn left then right during _searchTimeBetweenPoints seconds
            var playerStartRotation = transform.rotation;
            var playerLeftRotation = Quaternion.Euler(0, -45, 0);
            var playerRightRotation = Quaternion.Euler(0, 45, 0);

            var halfTime = _searchTimeBetweenPoints / 2;

            float elapsedTime = 0.0f;
            while (elapsedTime < halfTime)
            {
                float t = elapsedTime / (halfTime);
                transform.rotation = Quaternion.Slerp(playerStartRotation, playerLeftRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = playerLeftRotation;

            playerStartRotation = transform.rotation;

            elapsedTime = 0.0f;
            while (elapsedTime < halfTime)
            {
                float t = elapsedTime / (halfTime);
                transform.rotation = Quaternion.Slerp(playerStartRotation, playerRightRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = playerRightRotation;
        }

        private IEnumerator SearchBeforeContinueWaypoint()
        {
            yield return TurnLeftAndRight();

            ResumePatrol();

            // Hide suspicious icon
            _suspiciousIcon.SetActive(false);

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
            _navMeshAgent.SetDestination(newDestinationPosition);

            _searchCoroutine = null;
        }

        private void ResumePatrol()
        {
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].transform.position);

            _searchCoroutine = null;
        }
    }
}
