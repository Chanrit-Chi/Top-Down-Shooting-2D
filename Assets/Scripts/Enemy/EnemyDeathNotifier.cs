using UnityEngine;

namespace Enemy
{
    public class EnemyDeathNotifier : MonoBehaviour
    {
        private WaveManager waveManager;
        private AdvancedWaveManager advancedWaveManager;

        public void SetWaveManager(WaveManager manager)
        {
            waveManager = manager;
            advancedWaveManager = null;
        }

        public void SetWaveManager(AdvancedWaveManager manager)
        {
            advancedWaveManager = manager;
            waveManager = null;
        }

        public void NotifyDeath()
        {
            if (waveManager != null)
            {
                waveManager.OnEnemyDefeated();
            }
            else if (advancedWaveManager != null)
            {
                advancedWaveManager.OnEnemyDefeated();
            }
        }
    }
}
