using UnityEngine;
using TMPro;

namespace TopDown.UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI enemyCountText;

        private Enemy.AdvancedWaveManager waveManager;

        private void Start()
        {
            waveManager = FindFirstObjectByType<Enemy.AdvancedWaveManager>();
            if (waveManager == null)
            {
                Debug.LogError("WaveUI: AdvancedWaveManager not found in scene!");
                enabled = false;
            }
        }

        private void Update()
        {
            if (waveManager != null)
            {
                if (waveText != null)
                {
                    string waveValue = $"Wave: {waveManager.GetCurrentWave()}";
                    waveText.text = waveValue;
                    
                }
                else
                {
                    Debug.LogWarning("WaveUI: waveText is NULL!");
                }
                
                if (enemyCountText != null)
                {
                    string enemyValue = $"Enemies: {waveManager.GetEnemiesAlive()}";
                    enemyCountText.text = enemyValue;
                    Debug.Log($"WaveUI: Set enemyCountText to '{enemyValue}'");
                }
                else
                {
                    Debug.LogWarning("WaveUI: enemyCountText is NULL!");
                }
            }
            else
            {
                Debug.LogWarning("WaveUI: waveManager is NULL!");
            }
        }
    }
}
