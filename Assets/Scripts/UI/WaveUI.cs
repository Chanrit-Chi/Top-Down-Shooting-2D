using UnityEngine;
using TMPro;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI enemyCountText;

        private Enemy.WaveManager waveManager;

        private void Start()
        {
            waveManager = FindObjectOfType<Enemy.WaveManager>();
            if (waveManager == null)
            {
                Debug.LogError("WaveUI: WaveManager not found in scene!");
                enabled = false;
            }
        }

        private void Update()
        {
            if (waveManager != null)
            {
                waveText.text = $"Wave: {waveManager.GetCurrentWave()}";
                enemyCountText.text = $"Enemies: {waveManager.GetEnemiesAlive()}";
            }
        }
    }
}
