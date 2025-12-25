using TopDown.Movement;
using UnityEngine;

namespace TopDown.ItemPickup
{
    public class EnemyDropItem : MonoBehaviour
    {
        [Header("Drop Prefabs")]
        public GameObject healthPickupPrefab;
        public GameObject bulletPickupPrefab;

        [Header("Drop Chances %")]
        [Range(0, 100)] public float healthDropChance = 20;
        [Range(0, 100)] public float bulletDropChance = 30;

        public void DropItem()
        {
            float roll = Random.Range(0, 100);
            float currentHealth = FindFirstObjectByType<PlayerHealth>().GetCurrentHealth();

            // if (currentHealth < 30)
            // {
            //     healthDropChance = 60;
            // }
            if (roll < healthDropChance && healthPickupPrefab != null)
            {
                Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
            }
            else if (roll < healthDropChance + bulletDropChance && bulletPickupPrefab != null)
            {
                Instantiate(bulletPickupPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
