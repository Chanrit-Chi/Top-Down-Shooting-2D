using UnityEngine;

namespace Enemy
{
    [System.Serializable]
    public class EnemySpawnConfig
    {
        public GameObject enemyPrefab;
        [Range(1, 100)] public int count = 1;
    }

    [System.Serializable]
    public class WaveConfig
    {
        public int waveNumber;
        public EnemySpawnConfig[] enemyTypes; // Multiple enemy types per wave
        [Range(0.5f, 3f)] public float spawnDelayOverride = 0.5f;
    }

    public class AdvancedWaveManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs; // All available enemy types
        [SerializeField] private WaveConfig[] predefinedWaves; // Custom wave configurations
        [SerializeField] private bool usePredefinedWaves = false;
        [SerializeField] private bool infiniteWavesAfterPredefined = true; // Continue with scaled waves after predefined
        [SerializeField] private float delayBetweenWaves = 3f;
        [SerializeField] private int initialEnemiesPerWave = 3;
        [SerializeField] private float enemyHealthMultiplier = 1.1f;
        [SerializeField] private float enemyDamageMultiplier = 1.05f;
        [SerializeField] private bool randomizeEnemyTypesInUnlimited = true; // Mix enemy types in unlimited waves

        private AdvancedEnemySpawner enemySpawner;
        private int currentWave = 0;
        private int enemiesAlive = 0;
        private float waveTimer = 0f;
        private bool isWaveActive = false;

        private void Start()
        {
            enemySpawner = GetComponent<AdvancedEnemySpawner>();
            if (enemySpawner == null)
            {
                Debug.LogError("AdvancedWaveManager: AdvancedEnemySpawner component not found!");
                return;
            }
            
            StartNextWave();
        }

        private void Update()
        {
            if (!isWaveActive && waveTimer > 0)
            {
                waveTimer -= Time.deltaTime;
                if (waveTimer <= 0)
                {
                    StartNextWave();
                }
            }
        }

        private void StartNextWave()
        {
            currentWave++;
            isWaveActive = true;
            waveTimer = 0f;

            // Determine which wave type to spawn
            if (usePredefinedWaves && currentWave <= predefinedWaves.Length)
            {
                // Use predefined wave configuration
                SpawnPredefinedWave(predefinedWaves[currentWave - 1]);
            }
            else if (usePredefinedWaves && currentWave > predefinedWaves.Length && infiniteWavesAfterPredefined)
            {
                // After predefined waves, continue with scaled waves
                Debug.Log($"Predefined waves complete! Continuing with infinite scaled waves...");
                SpawnScaledWave();
            }
            else
            {
                // Use dynamic scaling (either from start or infinite mode)
                SpawnScaledWave();
            }
        }

        private void SpawnPredefinedWave(WaveConfig waveConfig)
        {
            Debug.Log($"=== WAVE {currentWave} STARTED (Predefined) ===");
            
            int totalEnemies = 0;
            foreach (var config in waveConfig.enemyTypes)
            {
                totalEnemies += config.count;
            }

            float healthMultiplier = Mathf.Pow(enemyHealthMultiplier, currentWave - 1);
            float damageMultiplier = Mathf.Pow(enemyDamageMultiplier, currentWave - 1);

            enemySpawner.SpawnCustomWave(waveConfig.enemyTypes, healthMultiplier, damageMultiplier, waveConfig.spawnDelayOverride);
            enemiesAlive = totalEnemies;
        }

        private void SpawnScaledWave()
        {
            int enemiesToSpawn = initialEnemiesPerWave + (currentWave - 1);
            float healthMultiplier = Mathf.Pow(enemyHealthMultiplier, currentWave - 1);
            float damageMultiplier = Mathf.Pow(enemyDamageMultiplier, currentWave - 1);

            Debug.Log($"=== WAVE {currentWave} STARTED ===");
            Debug.Log($"Enemies to spawn: {enemiesToSpawn} | Health: x{healthMultiplier:F2} | Damage: x{damageMultiplier:F2}");

            enemySpawner.SpawnWave(enemiesToSpawn, healthMultiplier, damageMultiplier, randomizeEnemyTypesInUnlimited);
            enemiesAlive = enemiesToSpawn;
        }

        public void OnEnemyDefeated()
        {
            enemiesAlive--;
            Debug.Log($"Enemy defeated! Remaining: {enemiesAlive}");

            if (enemiesAlive <= 0 && isWaveActive)
            {
                isWaveActive = false;
                waveTimer = delayBetweenWaves;
                Debug.Log($"Wave {currentWave} complete! Next wave in {delayBetweenWaves}s...");
            }
        }

        public int GetCurrentWave() => currentWave;
        public int GetEnemiesAlive() => enemiesAlive;
        public bool IsWaveActive() => isWaveActive;
    }
}
