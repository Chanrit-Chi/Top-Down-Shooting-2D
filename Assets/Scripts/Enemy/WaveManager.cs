using UnityEngine;
using System.Collections.Generic;

namespace Enemy
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private float delayBetweenWaves = 3f;
        [SerializeField] private int initialEnemiesPerWave = 3;
        [SerializeField] private float enemyHealthMultiplier = 1.1f; // 10% more health each wave
        [SerializeField] private float enemyDamageMultiplier = 1.05f; // 5% more damage each wave

        private int currentWave = 0;
        private int enemiesAlive = 0;
        private float waveTimer = 0f;
        private bool isWaveActive = false;

        private void Start()
        {
            if (enemySpawner == null)
            {
                enemySpawner = GetComponent<EnemySpawner>();
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

            int enemiesToSpawn = initialEnemiesPerWave + (currentWave - 1);
            float healthMultiplier = Mathf.Pow(enemyHealthMultiplier, currentWave - 1);
            float damageMultiplier = Mathf.Pow(enemyDamageMultiplier, currentWave - 1);

            Debug.Log($"=== WAVE {currentWave} STARTED ===");
            Debug.Log($"Enemies to spawn: {enemiesToSpawn} | Health: x{healthMultiplier:F2} | Damage: x{damageMultiplier:F2}");

            enemySpawner.SpawnWave(enemiesToSpawn, healthMultiplier, damageMultiplier);
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

        public int GetCurrentWave()
        {
            return currentWave;
        }

        public int GetEnemiesAlive()
        {
            return enemiesAlive;
        }

        public bool IsWaveActive()
        {
            return isWaveActive;
        }
    }
}
