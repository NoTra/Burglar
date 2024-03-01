using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class StopRotation : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // The circle should not rotate with the parent
            transform.rotation = Quaternion.Euler(90, -1 * transform.parent.rotation.y, 0);
        }
    }
}
