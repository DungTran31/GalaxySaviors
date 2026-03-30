using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyIceBullet : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [Range(1, 50)]
        [SerializeField] private float speed = 15f;
        [Range(1, 10)]
        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private float slowAmount = 50f; // Amount to slow the player
        [SerializeField] private float slowDuration = 2f; // Duration of the slow effect

        private Rigidbody2D rb;

        private void OnEnable()
        {
            // Ensure the Rigidbody2D component is assigned
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            // Restart the coroutine each time the bullet is activated
            StopAllCoroutines(); // Stop any existing coroutines to avoid duplicates
            StartCoroutine(ReturnToPoolAfterDelay());
        }

        private IEnumerator ReturnToPoolAfterDelay()
        {
            yield return new WaitForSeconds(lifeTime);
            this.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = transform.up * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.TryGetComponent<Player.PlayerHealth>(out var playerHealth))
                {
                    playerHealth.TakeDamage(damage);
                    playerHealth.ApplySlow(slowAmount, slowDuration);
                }
                this.gameObject.SetActive(false);
            }
            else if (collision.CompareTag("Border"))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
