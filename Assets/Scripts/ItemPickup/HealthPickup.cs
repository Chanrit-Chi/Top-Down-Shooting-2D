using TopDown.Movement;
using UnityEngine;

namespace TopDown.ItemPickup
{
    public class HealthPickup : Pickup
    {
        public int healthAmount = 25;
        protected override void OnPickup(GameObject player)
        {
            player.GetComponent<PlayerHealth>().Heal(healthAmount);
        }
    }
}
