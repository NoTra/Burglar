using System.Collections.Generic;
using UnityEngine;
using burglar.agent;

namespace burglar.managers
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private List<Agent> _agents = new();
        private Coroutine agentGoingToSwitch;

        private void OnEnable()
        {
            EventManager.LightChange +=  (switchGO) => SendClosestAgentToSwitch(switchGO);
        }

        private void OnDisable()
        {
            EventManager.LightChange -= (switchGO) => SendClosestAgentToSwitch(switchGO);
        }

        private void SendClosestAgentToSwitch(GameObject switchGO)
        {
            // Parse all agents and the closest one to the switch will check it
            Agent closestAgent = null;
            var closestDistance = float.MaxValue;
            foreach (var agent in _agents)
            {
                if (agent == null)
                {
                    continue;
                }
                
                var distance = Vector3.Distance(agent.transform.position, switchGO.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestAgent = agent;
                }
            }

            if (closestAgent == null)
            {
                return;
            }
            
            Debug.Log("Closest agent to switch is: " + closestAgent.name);

            var closestAgentPatrol = closestAgent.GetComponent<Patrol>();

            closestAgent.ChangeState(Agent.State.Suspicious);
            if (closestAgentPatrol != null)
            {
                closestAgentPatrol.GoToSwitch(switchGO);
            }
        }

        public void ResetAgents()
        {
            _agents.ForEach(agent => agent.Reset());
        }
    }
}
