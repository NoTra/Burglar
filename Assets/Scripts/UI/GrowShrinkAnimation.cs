using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace burglar
{
    public class GrowShrinkAnimation : MonoBehaviour
    {
        private Image targetImage;
        public float growDuration = 1f;  // Dur�e pour grandir
        public float shrinkDuration = 1f;  // Dur�e pour r�tr�cir
        public float maxScale = 1.5f;  // �chelle maximale
        public float minScale = 1f;  // �chelle minimale

        private Coroutine _animationCouroutine;

        void Start()
        {
            targetImage = GetComponent<Image>();

            if (targetImage != null && targetImage.enabled == true)
            {
                _animationCouroutine = StartCoroutine(AnimateImage());
            }
        }

        void OnDisable()
        {
            _animationCouroutine = null;
        }

        private void OnEnable()
        {
            if (targetImage != null && targetImage.enabled == true)
            {
                _animationCouroutine = StartCoroutine(AnimateImage());
            }
        }

        IEnumerator AnimateImage()
        {
            while (true)
            {
                // Grandir
                yield return StartCoroutine(ScaleImage(targetImage.transform, minScale, maxScale, growDuration));
                // R�tr�cir
                yield return StartCoroutine(ScaleImage(targetImage.transform, maxScale, minScale, shrinkDuration));
            }
        }

        IEnumerator ScaleImage(Transform target, float fromScale, float toScale, float duration)
        {
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                float newScale = Mathf.Lerp(fromScale, toScale, timeElapsed / duration);
                target.localScale = new Vector3(newScale, newScale, newScale);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            target.localScale = new Vector3(toScale, toScale, toScale);
        }
    }
}
