using System.Collections;
using UnityEngine;

namespace TopDown.ItemPickup
{
    
    public abstract class Pickup : MonoBehaviour
    {
        IEnumerator CollectionEffect()
        {
            float duration = 0;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color targetColor = new Color(0.922f, 0f, 0.2f, 1f); // rgba(235, 0, 51, 1)
            while (duration < 0.4)
            {
                duration += Time.deltaTime;
                spriteRenderer.color = new Color(targetColor.r, targetColor.g, targetColor.b, Mathf.Lerp(1, 0f, duration / 0.15f));
                yield return null;
            }

            Destroy(gameObject);
        }
        protected abstract void OnPickup(GameObject player);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnPickup(other.gameObject);
                StartCoroutine(CollectionEffect());
            }
        }
    }
}
