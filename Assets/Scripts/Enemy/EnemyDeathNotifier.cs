using UnityEngine;

namespace Enemy
{
    public class EnemyDeathNotifier : MonoBehaviour
    {

        private AdvancedWaveManager advancedWaveManager;

        public void SetWaveManager(AdvancedWaveManager manager)
        {
            advancedWaveManager = manager;
        }

        public void NotifyDeath()
        {
            if (advancedWaveManager != null)
            {
                advancedWaveManager.OnEnemyDefeated();
            }
        }
    }
}
