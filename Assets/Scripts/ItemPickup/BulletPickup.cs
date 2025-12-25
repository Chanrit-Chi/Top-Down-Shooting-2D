using TopDown.Shooting;
using UnityEngine;

namespace TopDown.ItemPickup
{
    public class BulletPickup : Pickup
    {
        public int bulletAmount = 25;
        protected override void OnPickup(GameObject player)
        {
            player.GetComponent<GunController>().AddBullets(bulletAmount);
        }
    }

}
