using UnityEngine;
using System.Collections.Generic;

namespace Enemy
{
    [RequireComponent(typeof(AdvancedWaveManager))]
    public class AdvancedEnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs; // Multiple enemy types
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private bool useRandomSpawning = true;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float randomSpawnRadius = 10f;
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private bool randomizeEnemyTypes = true;

        private AdvancedWaveManager waveManager;
        private int spawnPointIndex = 0;

        private void Awake()
        {
            waveManager = GetComponent<AdvancedWaveManager>();
            
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("AdvancedEnemySpawner: No enemy prefabs assigned!");
            }
            
            if (useRandomSpawning && spawnCenter == null)
            {
                spawnCenter = transform;
                Debug.Log("AdvancedEnemySpawner: Using position as spawn center");
            }
        }

        public void SpawnWave(int enemyCount, float healthMultiplier, float damageMultiplier, bool randomizeTypes = true)
        {
            StartCoroutine(SpawnEnemiesCoroutine(enemyCount, healthMultiplier, damageMultiplier, spawnDelay, randomizeTypes));
        }

        public void SpawnCustomWave(EnemySpawnConfig[] configs, float healthMultiplier, float damageMultiplier, float delayOverride)
        {
            StartCoroutine(SpawnCustomEnemiesCoroutine(configs, healthMultiplier, damageMultiplier, delayOverride));
        }

        private System.Collections.IEnumerator SpawnEnemiesCoroutine(int enemyCount, float healthMultiplier, float damageMultiplier, float delay, bool randomizeTypes)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy(null, healthMultiplier, damageMultiplier, randomizeTypes);
                yield return new WaitForSeconds(delay);
            }
        }

        private System.Collections.IEnumerator SpawnCustomEnemiesCoroutine(EnemySpawnConfig[] configs, float healthMultiplier, float damageMultiplier, float delay)
        {
            foreach (var config in configs)
            {
                for (int i = 0; i < config.count; i++)
                {
                    SpawnEnemy(config.enemyPrefab, healthMultiplier, damageMultiplier);
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        private void SpawnEnemy(GameObject prefabOverride, float healthMultiplier, float damageMultiplier, bool randomizeTypes = false)
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("AdvancedEnemySpawner: No enemy prefabs assigned!");
                return;
            }

            // Select enemy type
            GameObject enemyPrefab = prefabOverride != null
                ? prefabOverride
                : (randomizeTypes 
                    ? enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]
                    : enemyPrefabs[0]);

            Vector3 spawnPosition;

            if (useRandomSpawning)
            {
                Vector2 randomOffset = Random.insideUnitCircle * randomSpawnRadius;
                spawnPosition = spawnCenter.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            }
            else
            {
                if (spawnPoints.Length == 0)
                {
                    Debug.LogError("AdvancedEnemySpawner: No spawn points!");
                    return;
                }
                
                Transform spawnPoint = spawnPoints[spawnPointIndex % spawnPoints.Length];
                spawnPosition = spawnPoint.position;
                spawnPointIndex++;
            }

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.SetHealthMultiplier(healthMultiplier);
            }

            EnemyAttack enemyAttack = newEnemy.GetComponent<EnemyAttack>();
            if (enemyAttack != null)
            {
                enemyAttack.SetDamageMultiplier(damageMultiplier);
            }

            EnemyDeathNotifier deathNotifier = newEnemy.GetComponent<EnemyDeathNotifier>();
            if (deathNotifier != null)
            {
                deathNotifier.SetWaveManager(waveManager);
            }

            Debug.Log($"Spawned {enemyPrefab.name} (HP: x{healthMultiplier:F2}, DMG: x{damageMultiplier:F2})");
        }
    }
}
