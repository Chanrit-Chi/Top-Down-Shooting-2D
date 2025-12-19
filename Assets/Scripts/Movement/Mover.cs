using UnityEngine;

namespace TopDown.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private Rigidbody2D body;
        protected Vector3 currentInput;
        public Vector3 CurrentInput => currentInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            body.MovePosition(body.position + new Vector2(currentInput.x, currentInput.y) * moveSpeed * Time.fixedDeltaTime);
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
