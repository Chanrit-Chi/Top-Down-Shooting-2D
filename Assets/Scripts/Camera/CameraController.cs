using UnityEngine;
using UnityEngine.InputSystem;


namespace TopDown.CameraController
{       
    public class CameraController : MonoBehaviour
    {

        [SerializeField]
        private Transform playerTransform;
        [SerializeField]
        private float displacementMultiplier = 0.15f;
        private float zPosition = -10f;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
            // Don't move camera when game is paused
            if (Time.timeScale == 0f)
            {
                return;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 cameraDisplacement = (mousePosition - playerTransform.position) * displacementMultiplier;
            Vector3 finalCameraPosition = new Vector3(playerTransform.position.x + cameraDisplacement.x, playerTransform.position.y + cameraDisplacement.y, zPosition);
            transform.position = finalCameraPosition;
            transform.position = new Vector3(playerTransform.position.x + cameraDisplacement.x, playerTransform.position.y + cameraDisplacement.y, zPosition);
        }
    }
}

