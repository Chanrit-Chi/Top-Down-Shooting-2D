using UnityEngine;
using System.Collections.Generic;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs; // Multiple enemy types
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private bool useRandomSpawning = true;
        [SerializeField] private Transform[] spawnPoints; // Optional, for fixed spawn points
        [SerializeField] private float randomSpawnRadius = 10f; // For random spawning
        [SerializeField] private Transform spawnCenter; // Center point for random spawning
        [SerializeField] private bool randomizeEnemyTypes = true; // Randomly pick from enemy types

        private WaveManager waveManager;
        private int spawnPointIndex = 0;

        private void Awake()
        {
            waveManager = GetComponent<WaveManager>();
            
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("EnemySpawner: No enemy prefabs assigned!");
            }
            
            if (useRandomSpawning)
            {
                if (spawnCenter == null)
                {
                    spawnCenter = transform;
                    Debug.Log("EnemySpawner: Using WaveManager position as spawn center");
                }
            }
            else if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("EnemySpawner: No spawn points assigned and random spawning disabled!");
            }
        }

        public void SpawnWave(int enemyCount, float healthMultiplier, float damageMultiplier)
        {
            StartCoroutine(SpawnEnemiesCoroutine(enemyCount, healthMultiplier, damageMultiplier));
        }

        private System.Collections.IEnumerator SpawnEnemiesCoroutine(int enemyCount, float healthMultiplier, float damageMultiplier)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy(healthMultiplier, damageMultiplier);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private void SpawnEnemy(float healthMultiplier, float damageMultiplier)
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("Cannot spawn enemy - no enemy prefabs assigned!");
                return;
            }

            // Select enemy type
            GameObject enemyPrefab = randomizeEnemyTypes 
                ? enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]
                : enemyPrefabs[0]; // If not randomizing, always spawn first type

            Vector3 spawnPosition;

            if (useRandomSpawning)
            {
                // Random position in a circle around spawn center
                Vector2 randomOffset = Random.insideUnitCircle * randomSpawnRadius;
                spawnPosition = spawnCenter.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            }
            else
            {
                // Use fixed spawn points
                if (spawnPoints.Length == 0)
                {
                    Debug.LogError("Cannot spawn enemy - no spawn points!");
                    return;
                }
                
                Transform spawnPoint = spawnPoints[spawnPointIndex % spawnPoints.Length];
                spawnPosition = spawnPoint.position;
                spawnPointIndex++;
            }

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            // Apply difficulty scaling
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

            // Register enemy with wave manager
            EnemyDeathNotifier deathNotifier = newEnemy.GetComponent<EnemyDeathNotifier>();
            if (deathNotifier != null)
            {
                deathNotifier.SetWaveManager(waveManager);
            }

            Debug.Log($"Spawned {enemyPrefab.name} at {spawnPosition} (HP: x{healthMultiplier:F2}, DMG: x{damageMultiplier:F2})");
        }
    }
}


