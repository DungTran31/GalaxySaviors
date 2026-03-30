using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [Range(1, 50)]
        [SerializeField] private float speed = 15f;
        [Range(1, 10)]
        [SerializeField] private float lifeTime = 2f;

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
                collision.GetComponent<Player.PlayerHealth>().TakeDamage(damage);
                this.gameObject.SetActive(false);
            }
            else if (collision.CompareTag("Border"))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
