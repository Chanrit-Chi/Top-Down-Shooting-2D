using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealthSystem
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player health: {currentHealth}/{maxHealth}");

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
        Debug.Log("Player died!");
        // TODO: Add game over logic here
        Destroy(gameObject);
    }
}
