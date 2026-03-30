using DungTran31.GamePlay.Player.SO;
using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Player
{
    public class BlackBullet : MonoBehaviour
    {
        [SerializeField] private PlayerBulletSO playerBulletSO;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void OnEnable()
        {
            // Restart the coroutine each time the bullet is activated
            StopAllCoroutines(); // Stop any existing coroutines to avoid duplicates
            StartCoroutine(ReturnToPoolAfterDelay());
        }

        private IEnumerator ReturnToPoolAfterDelay()
        {
            yield return new WaitForSeconds(playerBulletSO.LifeTime);
            this.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = transform.right * playerBulletSO.Speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<Enemy.EnemyHealth>(out Enemy.EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(playerBulletSO.Damage);
                }
                else if (collision.TryGetComponent<Enemy.BossHealth>(out Enemy.BossHealth bossHealth))
                {
                    bossHealth.TakeDamage(playerBulletSO.Damage);
                }
                this.gameObject.SetActive(false);
            }
            else if(collision.CompareTag("Border"))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
