using UnityEngine;
using burglar.managers;

namespace burglar.utility
{
    public class Selectable : MonoBehaviour
    {
        public object element;
        public void Decide()
        {
            DialogManager.SetDecision(element);
        }
    }
}
