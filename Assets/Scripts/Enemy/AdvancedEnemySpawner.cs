using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Enemy
{
    [RequireComponent(typeof(AdvancedWaveManager))]
    public class AdvancedEnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs; // Multiple enemy types
        [SerializeField] private float spawnDelay = 1.5f;
        [SerializeField] private bool useRandomSpawning = true;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float randomSpawnRadius = 2f;
        [SerializeField] private Transform spawnCenter;
        [SerializeField] public Tilemap groundTilemap; // Tilemap for spawning

        private AdvancedWaveManager waveManager;
        private int spawnPointIndex = 0;

        private void Awake()
        {
            waveManager = GetComponent<AdvancedWaveManager>();
            
            // Auto-find tilemap if not assigned
            if (groundTilemap == null)
            {
                Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
                foreach (Tilemap tilemap in tilemaps)
                {
                    if (tilemap.gameObject.name == "Ground")
                    {
                        groundTilemap = tilemap;
                        Debug.Log("AdvancedEnemySpawner: Found 'Ground' Tilemap");
                        break;
                    }
                }
                
                if (groundTilemap == null && tilemaps.Length > 0)
                {
                    groundTilemap = tilemaps[0];
                    Debug.LogWarning($"AdvancedEnemySpawner: 'Ground' tilemap not found, using first available: {groundTilemap.gameObject.name}");
                }
                
                if (groundTilemap == null)
                {
                    Debug.LogError("AdvancedEnemySpawner: No Tilemap found in scene!");
                }
            }
            
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

            if (useRandomSpawning && groundTilemap != null)
            {
                spawnPosition = GetTilemapSpawnPosition();
            }
            else if (useRandomSpawning)
            {
                Vector2 randomOffset = Random.insideUnitCircle * randomSpawnRadius;
                spawnPosition = spawnCenter.position + new Vector3(randomOffset.x, randomOffset.y, 0);
                Debug.LogWarning($"AdvancedEnemySpawner: Using circle spawn (no tilemap) at {spawnPosition}");
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

            Debug.Log($"Spawned {enemyPrefab.name} at {spawnPosition} (HP: x{healthMultiplier:F2}, DMG: x{damageMultiplier:F2})");
        }

        private Vector3 GetTilemapSpawnPosition()
        {
            if (groundTilemap == null)
            {
                Debug.LogError("AdvancedEnemySpawner: groundTilemap is NULL!");
                return spawnCenter.position;
            }
            
            BoundsInt bounds = groundTilemap.cellBounds;
            
            // Collect all valid tile positions
            List<Vector3Int> validTiles = new List<Vector3Int>();
            foreach (var pos in bounds.allPositionsWithin)
            {
                if (groundTilemap.HasTile(pos))
                {
                    validTiles.Add(pos);
                }
            }
            
            if (validTiles.Count == 0)
            {
                Debug.LogError($"AdvancedEnemySpawner: No valid tiles found in tilemap '{groundTilemap.name}'!");
                return spawnCenter.position;
            }
            
            // Pick a random valid tile
            Vector3Int randomTile = validTiles[Random.Range(0, validTiles.Count)];
            Vector3 spawnPos = groundTilemap.GetCellCenterWorld(randomTile);
            return spawnPos;
        }
    }
}
