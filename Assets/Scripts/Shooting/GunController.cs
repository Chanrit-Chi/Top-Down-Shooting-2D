using TopDown.Audio;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Shooting
{  
    public class GunController : MonoBehaviour
    {
        [Header("Cooldown")]
        [SerializeField] private float shootCooldown = 0.25f;
        private float CooldownTimer;

        [Header("Reload")]
        [SerializeField] private float reloadTime = 1.5f;
        private float reloadTimer;
        private bool isReloading;

        [Header("References")]
        [SerializeField] private Animator MuzzleFlashAnimator;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioClip reloadSound;
        [SerializeField] private AudioClip emptySound;
        
        [Header("Ammo")]
        [SerializeField]private int InitialAmmo;
        [SerializeField] private int clipSize;



        public IntReactiveProperty totalAmmo { get; private set;} = new IntReactiveProperty(0);
        public IntReactiveProperty currentAmmoInClip { get; private set;} = new IntReactiveProperty(0);
        
        private void Awake()
        {
            totalAmmo.Value = InitialAmmo;

            // Load a full clip (but not more than the total ammo available)
            if (clipSize <= InitialAmmo)
            {
                currentAmmoInClip.Value = clipSize;
            }
            else
            {
                currentAmmoInClip.Value = InitialAmmo;
            }
        }

        private void Update()
        {
            CooldownTimer += Time.deltaTime;
            
            // Handle reload timer
            if (isReloading)
            {
                reloadTimer += Time.deltaTime;
                if (reloadTimer >= reloadTime)
                {
                    // Reload complete - refill the clip
                    CompleteReload();
                }
            }
        }
        public void Shoot()
        {
            if (CooldownTimer < shootCooldown) return;
            if (isReloading) return; // Can't shoot while reloading
            
            // Auto-reload if clip is empty but there's ammo available
            if (currentAmmoInClip.Value <= 0)
            {
                SoundManager.Instance?.PlaySound(emptySound);
                if (totalAmmo.Value > 0)
                {
                    Reload();
                    return;
                }
                else
                {
                    return;
                }
            }
            
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, null);
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.ShootBullet(firePoint);
            if (MuzzleFlashAnimator != null){
                Debug.Log("Play Muzzle Flash Animation");
                MuzzleFlashAnimator.SetTrigger("shoot");
            }
            else {
                Debug.Log("No Muzzle Flash Animator is null");
            }
            CooldownTimer = 0f;
            currentAmmoInClip.Value -= 1;

            // Play shooting sound
            SoundManager.Instance?.PlaySound(shootSound);


        }

        public void AddBullets(int amount)
        {
            
            totalAmmo.Value += amount;
            Debug.Log("Added " + amount + " bullets. Total ammo: " + totalAmmo.Value);
        }

        private void Reload()
        {
            // Don't reload if already reloading or clip is full
            if (isReloading) return;
            if (currentAmmoInClip.Value == clipSize) return;
            if (totalAmmo.Value <= 0) return;
            
            // Start reload
            isReloading = true;
            reloadTimer = 0f;
            Debug.Log("Reloading... (takes " + reloadTime + " seconds)");
            // Play reload sound
            SoundManager.Instance?.PlaySound(reloadSound);
        }

        private void CompleteReload()
        {
            // Calculate how much ammo to add
            int ammoNeeded = clipSize - currentAmmoInClip.Value;
            
            if (totalAmmo.Value >= ammoNeeded)
            {
                // Enough ammo to fully reload
                currentAmmoInClip.Value += ammoNeeded;
                totalAmmo.Value -= ammoNeeded;
            }
            else
            {
                // Not enough ammo to fully reload
                currentAmmoInClip.Value += totalAmmo.Value;
                totalAmmo.Value = 0;
            }
            
            // Reload complete
            isReloading = false;
            reloadTimer = 0f;
            Debug.Log("Reload complete!");
        }

        #region Input

        private void OnShoot()
        {
            Shoot();
        }

        private void OnReload()
        {
            Reload();
        }
        #endregion
    }
}
