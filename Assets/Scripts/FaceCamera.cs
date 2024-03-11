using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class FaceCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        void Start()
        {
        
        }

        void Update()
        {
            // Face the camera
            transform.LookAt(transform.position + _cameraTransform.forward, _cameraTransform.up);
        }
    }
}
