using UnityEngine;
using System.Collections;

namespace Benjathemaker
{
    public class SimpleGemsAnim : MonoBehaviour
    {
        public bool isRotating = false;
        public bool rotateX = false;
        public bool rotateY = false;
        public bool rotateZ = false;
        public float rotationSpeed = 90f; // Degrees per second
        public float verticalRotationMultiplier = 0.5f; // Now added for less vertical rotation

        public bool isFloating = false;
        public bool useEasingForFloating = false;
        public float floatHeight = 1f;
        public float floatSpeed = 1f;
        private Vector3 initialPosition;
        private float floatTimer;

        private Vector3 initialScale;
        public Vector3 startScale;
        public Vector3 endScale;

        public bool isScaling = false;
        public bool useEasingForScaling = false;
        public float scaleLerpSpeed = 1f;
        private float scaleTimer;

        void Start()
        {
            initialScale = transform.localScale;
            initialPosition = transform.position;

            startScale = initialScale;
            endScale = initialScale * 0.2f; // Only scale to 20% of original
        }

        void Update()
        {
            if (isRotating)
            {
                Vector3 rotationVector = new Vector3(
                    rotateX ? verticalRotationMultiplier : 0,
                    rotateY ? 1 : 0,
                    rotateZ ? 1 : 0
                );
                transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
            }

            if (isFloating)
            {
                floatTimer += Time.deltaTime * floatSpeed;
                float t = Mathf.PingPong(floatTimer, 1f);
                if (useEasingForFloating) t = EaseInOutQuad(t);

                transform.position = initialPosition + new Vector3(0, t * floatHeight, 0);
            }

            if (isScaling)
            {
                scaleTimer += Time.deltaTime * scaleLerpSpeed;
                float t = Mathf.PingPong(scaleTimer, 1f);

                if (useEasingForScaling)
                {
                    t = EaseInOutQuad(t);
                }

                transform.localScale = Vector3.Lerp(startScale, endScale, t);
            }
        }

        float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
        }
    }
}
