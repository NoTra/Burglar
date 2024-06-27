using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar.UI
{
    public class StopRotation : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            // The circle should not rotate with the parent
            transform.rotation = Quaternion.Euler(90, -1 * transform.parent.rotation.y, 0);
        }
    }
}
