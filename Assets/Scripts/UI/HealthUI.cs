using UnityEngine;
using TMPro;
using Enemy;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    
    private PlayerHealth playerHealth;
    private EnemyHealth[] enemyHealths;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        
        // Find all enemies
        enemyHealths = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (playerHealth != null && playerHealthText != null)
        {
            playerHealthText.text = $"{playerHealth.GetCurrentHealth():F0}/{playerHealth.GetMaxHealth():F0}";
        }

        if (enemyHealthText != null && enemyHealths.Length > 0)
        {
            float totalEnemyHealth = 0;
            foreach (var enemy in enemyHealths)
            {
                if (enemy != null)
                    totalEnemyHealth += enemy.GetCurrentHealth();
            }
            enemyHealthText.text = $"{enemyHealths.Length}";
        }
    }
}
