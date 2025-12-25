using UnityEngine;

namespace TopDown.ItemPickup
{
    public class FloatingPickup : MonoBehaviour
    {
        [Header("Floating")]
        public float floatSpeed = 2f;
        public float floatHeight = 0.25f;

        [Header("Rotation")]
        public float rotateSpeed = 45f;

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position;
        }

        void Update()
        {
            // Floating up and down
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(startPos.x, newY, startPos.z);

            // Rotate slowly
            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }
}
