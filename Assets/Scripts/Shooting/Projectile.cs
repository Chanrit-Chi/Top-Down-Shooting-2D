using UnityEngine;
namespace TopDown.Shooting
{   
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Movement State")]
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifeTime = 2f;
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

    }
}
