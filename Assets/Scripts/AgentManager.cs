using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private List<Agent> _agents = new();

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
            float closestDistance = float.MaxValue;
            foreach (Agent agent in _agents)
            {
                if (agent == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(agent.transform.position, switchGO.transform.position);
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

            Patrol closestAgentPatrol = closestAgent.GetComponent<Patrol>();

            closestAgent.ChangeState(Agent.State.Suspicious);
            closestAgentPatrol?.GoToSwitch(switchGO);
        }
    }
}
