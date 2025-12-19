using TopDown.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Movement
{
    public class PlayerRotation : Rotator
    {
        [Header("Body & Legs")]
        [SerializeField] private Transform body;
        [SerializeField] private Transform legs;

        [Header ("Mover reference")]
        [SerializeField] private Mover playerMover;
        private Vector3 CurrentInput => playerMover.CurrentInput;

        private void Update()
        {
            // Continuously rotate body towards mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), Camera.main.nearClipPlane));
            mousePosition.z = 0;
            lookAt(body, mousePosition);
            
            // Rotate legs towards movement direction
            if (playerMover.CurrentInput.magnitude > 0)
            {
                Vector3 legsLookTarget = transform.position + new Vector3(playerMover.CurrentInput.x, playerMover.CurrentInput.y);
                lookAt(legs, legsLookTarget);
            }
        }
    }
}
