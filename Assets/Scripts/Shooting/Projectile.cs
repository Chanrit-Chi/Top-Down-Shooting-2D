using UnityEngine;
namespace TopDown.Shooting
{   
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Movement State")]
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private float damageAmount = 20f;
        private Rigidbody2D rb;
        private float lifeTimer;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void ShootBullet(Transform shootPoint)
        {
            lifeTimer = 0f;
            rb.linearVelocity = Vector2.zero;
            transform.position = shootPoint.position;
            transform.rotation = shootPoint.rotation;
            gameObject.SetActive(true);

            rb.AddForce(-transform.up * speed, ForceMode2D.Impulse);
        } 

        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                IHealthSystem healthSystem = collision.GetComponent<IHealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.TakeDamage(damageAmount);
                    gameObject.SetActive(false);
                }
            }
        }

    }
}
