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

        [SerializeField] private GameObject _centerOfArea;

        private Agent _agent;
        private NavMeshAgent _navMeshAgent;
        public float _searchTimeBetweenPoints;
        private Coroutine _searchCoroutine;

        private Vector3 _suspiciousPoint;

        [SerializeField] private Animator _agentAnimator;

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
            EventManager.SoundGenerated += (point, strength, checkDistance) => CheckSuspiciousPoint(point, checkDistance);
        }

        private void OnDisable()
        {
            EventManager.SoundGenerated -= (point, strength, checkDistance) => CheckSuspiciousPoint(point, checkDistance);
        }

        private void CheckSuspiciousPoint(Vector3 point, bool checkDistance = true)
        {
            // Check if the point is in range of agent's earing radius
            if (checkDistance && Vector3.Distance(transform.position, point) > _agent._earingDistance)
            {
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

                _agent.ChangeState(Agent.State.Suspicious);

                if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
                {
                    _suspiciousPoint = Vector3.zero;

                    _searchCoroutine = StartCoroutine(SearchBeforeContinueWaypoint());
                }
                return;
            }

            if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance && _searchCoroutine == null)
            {
                // We change the next waypoint
                _currentWaypointIndex++;

                if (_currentWaypointIndex >= _waypoints.Count)
                {
                    _currentWaypointIndex = 0;
                }

                // Starts to search (left and right)
                _searchCoroutine = StartCoroutine(SearchBeforeNextWaypoint());
            }
        }

        private IEnumerator SearchBeforeNextWaypoint()
        {
            _agentAnimator.SetBool("isWalking", false);
            // Rotate player to LookAt the _centerOfArea
            var playerStartRotation = transform.rotation;

            var destinationPosition = new Vector3(_centerOfArea.transform.position.x, transform.position.y, _centerOfArea.transform.position.z);

            var destinationRotation = Quaternion.LookRotation(destinationPosition - transform.position);

            float elapsedTime = 0f;
            float duration = 1f;
            while (elapsedTime < duration)
            {
                transform.rotation = Quaternion.Slerp(playerStartRotation, destinationRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = destinationRotation;

            yield return TurnLeftAndRight();

            GoToNextWaypoint();
        }

        private IEnumerator TurnLeftAndRight()
        {
            _agentAnimator.SetTrigger("Search");

            // Turn left then right during _searchTimeBetweenPoints seconds
            var playerStartRotation = transform.rotation;
            var playerLeftRotation = Quaternion.Euler(0, -45, 0) * playerStartRotation;
            var playerRightRotation = Quaternion.Euler(0, 45, 0) * playerStartRotation;

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

            _agentAnimator.SetBool("isWalking", true);
        }

        private IEnumerator SearchBeforeContinueWaypoint()
        {
            yield return TurnLeftAndRight();

            ResumePatrol();

            _agent.ChangeState(Agent.State.Patrol);

            yield return null;
        }

        private void GoToNextWaypoint()
        {
            var newDestinationGO = _waypoints[_currentWaypointIndex];
            var newDestinationPosition = new Vector3(newDestinationGO.transform.position.x, newDestinationGO.transform.position.y, newDestinationGO.transform.position.z);
            _navMeshAgent.SetDestination(newDestinationPosition);

            _searchCoroutine = null;
        }

        private void ResumePatrol()
        {
            _agent.ChangeState(Agent.State.Patrol);
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].transform.position);

            _searchCoroutine = null;
        }

        public void GoToSwitch(GameObject switchGO)
        {
            StartCoroutine(GoToSwitchCoroutine(switchGO));
        }

        /// <summary>
        /// The agent walk to the switch, switch on the light, 
        /// look at the light, then switch off the light, 
        /// then switch on the light and go back to his patrol.
        /// </summary>
        /// <returns>void</returns>
        private IEnumerator GoToSwitchCoroutine(GameObject switchGO)
        {
            _navMeshAgent.SetDestination(switchGO.transform.position);

            while (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
            {
                yield return null;
            }

            var switchComponent = switchGO.GetComponent<LightSwitch>();
            switchComponent.TurnOnLight();

            yield return new WaitForSeconds(1.0f);

            float elapsedTime = 0;
            float rotationDuration = 0.8f;

            var startRotation = _navMeshAgent.transform.rotation;
            // We lock the y axis to avoid the agent to look up or down
            var lightPosition = new Vector3(switchComponent._light.transform.position.x, _navMeshAgent.transform.position.y, switchComponent._light.transform.position.z);
            var destinationRotation = Quaternion.LookRotation(lightPosition - _navMeshAgent.transform.position);

            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, destinationRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = destinationRotation;

            switchComponent.TurnOffLight();

            yield return new WaitForSeconds(1.0f);

            switchComponent.TurnOnLight();

            yield return TurnLeftAndRight();

            ResumePatrol();
        }
    }
}
