using UnityEngine;
using TMPro;
using Enemy;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI waveText;
    
    private PlayerHealth playerHealth;
    private EnemyHealth[] enemyHealths;
    private AdvancedWaveManager advancedWaveManager;
    private WaveManager waveManager;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        
        // Try to find AdvancedWaveManager first, then fall back to WaveManager
        advancedWaveManager = FindObjectOfType<AdvancedWaveManager>();
        if (advancedWaveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
        }
        
        // Find all enemies
        enemyHealths = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (playerHealth != null && playerHealthText != null)
        {
            playerHealthText.text = $"{playerHealth.GetCurrentHealth():F0}/{playerHealth.GetMaxHealth():F0}";
        }

        // Refresh enemy list every frame to account for newly spawned enemies
        enemyHealths = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
        
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

        if (waveText != null)
        {
            if (advancedWaveManager != null)
            {
                waveText.text = $"Wave: {advancedWaveManager.GetCurrentWave()}";
            }
            else if (waveManager != null)
            {
                waveText.text = $"Wave: {waveManager.GetCurrentWave()}";
            }
        }
    }
}
