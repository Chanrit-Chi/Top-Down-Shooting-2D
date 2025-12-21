using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealthSystem
    {
        [SerializeField] private float maxHealth = 30f;
        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"Enemy health: {currentHealth}/{maxHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        private void Die()
        {
            Debug.Log("Enemy defeated!");
            Destroy(gameObject);
        }
    }
}
