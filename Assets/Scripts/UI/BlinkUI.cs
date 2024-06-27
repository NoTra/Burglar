using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace burglar
{
    public class BlinkUI : MonoBehaviour
    {
        public float blinkInterval = 0.5f; // Interval between blinks in seconds
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            if (_image != null)
            {
                StartCoroutine(Blink());
            }
            else
            {
                Debug.LogError("No Image component found on this GameObject.");
            }
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                _image.enabled = !_image.enabled; // Toggle the visibility
                yield return new WaitForSeconds(blinkInterval);
            }
        }
    }
}
