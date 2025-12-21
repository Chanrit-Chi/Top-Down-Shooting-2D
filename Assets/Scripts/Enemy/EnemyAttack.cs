using UnityEngine;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private float damageAmount = 10f;
        [SerializeField] private float attackCooldown = 1f;
        
        private EnemyAwareController enemyAwareController;
        private float attackTimer;

        private void Awake()
        {
            enemyAwareController = GetComponent<EnemyAwareController>();
        }

        private void Update()
        {
            attackTimer -= Time.deltaTime;

            if (enemyAwareController.awareOfPlayer && 
                enemyAwareController.DistanceToPlayer <= attackRange && 
                attackTimer <= 0)
            {
                AttackPlayer();
            }
        }

        private void AttackPlayer()
        {
            // Get the player
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                // Try to get a health component
                IHealthSystem healthSystem = playerObject.GetComponent<IHealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.TakeDamage(damageAmount);
                    Debug.Log($"Enemy dealt {damageAmount} damage to player!");
                }
            }

            attackTimer = attackCooldown;
        }
    }
}
