using TopDown.Audio;
using TopDown.ItemPickup;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealthSystem
    {
        [SerializeField] private float maxHealth = 30f;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip deathSound;

        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void SetHealthMultiplier(float multiplier)
        {
            maxHealth *= multiplier;
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            SoundManager.Instance.PlaySound(damageSound);
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
            SoundManager.Instance.PlaySound(deathSound);
            
            // Notify wave manager
            EnemyDeathNotifier deathNotifier = GetComponent<EnemyDeathNotifier>();
            if (deathNotifier != null)
            {
                deathNotifier.NotifyDeath();
            }

            GetComponent<EnemyDropItem>()?.DropItem();
            
            Destroy(gameObject);
        }
    }
}
