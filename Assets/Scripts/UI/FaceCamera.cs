using UnityEngine;

namespace burglar.UI
{
    public class FaceCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Update()
        {
            if (Camera.main != null)
            {
                // Faire face � la cam�ra principale
                transform.LookAt(transform.position + Camera.main.transform.forward, Camera.main.transform.up);
            }
        }
    }
}
