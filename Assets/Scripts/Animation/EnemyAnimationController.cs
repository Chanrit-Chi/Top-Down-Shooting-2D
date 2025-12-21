using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField] private float attackDuration = 0.5f;
        
        private Animator animator;
        private EnemyMovement enemyMovement;
        private EnemyAttack enemyAttack;
        private EnemyAwareController enemyAwareController;
        
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private bool isCurrentlyAttacking = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyAttack = GetComponent<EnemyAttack>();
            enemyAwareController = GetComponent<EnemyAwareController>();
        }

        private void Update()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            // Check if enemy is moving
            bool isMoving = enemyMovement != null && enemyMovement.GetCurrentVelocity().magnitude > 0.1f;
            animator.SetBool(IsMoving, isMoving);

            // Check if enemy is attacking
            bool isInAttackRange = enemyAwareController != null && 
                                   enemyAwareController.DistanceToPlayer <= 0.5f;
            
            if (isInAttackRange && !isCurrentlyAttacking)
            {
                TriggerAttackAnimation();
            }
        }

        private void TriggerAttackAnimation()
        {
            isCurrentlyAttacking = true;
            animator.SetBool(IsAttacking, true);
            Invoke(nameof(StopAttackAnimation), attackDuration);
        }

        private void StopAttackAnimation()
        {
            isCurrentlyAttacking = false;
            animator.SetBool(IsAttacking, false);
        }

        // Public method to set attack duration if needed
        public void SetAttackDuration(float duration)
        {
            attackDuration = duration;
        }
    }
}
