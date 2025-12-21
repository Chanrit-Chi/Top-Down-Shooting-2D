using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float rotationSpeed;

        private Rigidbody2D rb;
        private Vector2 movementDirection;
        private EnemyAwareController enemyAwareController;
        private bool isTouchingPlayer = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemyAwareController  = GetComponent<EnemyAwareController>();
        }

        private void FixedUpdate()
        {
            updateTargetDirection();
            rotateTowardTarget();
            setVelocity();
        }

        private void updateTargetDirection()
        {
            // enemyAwareController  = GetComponent<EnemyAwareController>();
            if (enemyAwareController.awareOfPlayer)
            {
                movementDirection = enemyAwareController.DirectionToPlayer;
            }
            else
            {
                movementDirection = Vector2.zero;
            }
        }

        private void rotateTowardTarget()
        {
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = rb.rotation;
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            
            rb.MoveRotation(newAngle); 
        }


        private void setVelocity()
        {
            if (movementDirection == Vector2.zero)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                float distanceToPlayer = enemyAwareController.DistanceToPlayer;
                
                // Stop when touching player
                if (isTouchingPlayer && distanceToPlayer < 0.5f)
                {
                    rb.linearVelocity = Vector2.zero;
                }
                // Resume chasing if player moves away
                else if (distanceToPlayer > 0.3f)
                {
                    // Move directly towards the player
                    rb.linearVelocity = movementDirection * moveSpeed;
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }
}
