using UnityEngine;

using burglar.managers;

namespace burglar
{
    public class UIObjectives : MonoBehaviour
    {
        private void Start()
        {
            EventManager.OnObjectiveLoaded();
        }
    }
}
