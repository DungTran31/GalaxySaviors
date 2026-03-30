using DungTran31.GamePlay.Player.SO;
using System.Collections.Generic;
using UnityEngine;

namespace DungTran31.GamePlay.Player
{
    public class PlayerBall : MonoBehaviour
    {
        [SerializeField] private float ORBITAL_SPEED = 1f; // Speed of the circular motion
        [SerializeField] private float ORBITAL_RADIUS = 20f; // Desired radius around the player
        [SerializeField] private float RANDOM_OFFSET = 1f; // Random offset for more random movement
        [SerializeField] private float SMOOTHING_FACTOR = 0.1f; // Smoothing factor for interpolation
        [SerializeField] private float damage = 100f;

        public static List<PlayerBall> Bodies;

        public Rigidbody2D rb { get; protected set; }
        private Transform playerTransform;
        private float angle;
        private Vector2 targetPosition;

        void Start()
        {
            if (Bodies == null)
                Bodies = new List<PlayerBall>();

            Bodies.Add(this);

            rb = GetComponent<Rigidbody2D>();
            playerTransform = GameObject.FindWithTag("Player").transform; // Assuming the player has the tag "Player"
            angle = Random.Range(0f, 360f); // Start at a random angle
            UpdateTargetPosition();

            IgnoreCollisionWithPlayer(rb);
        }

        void FixedUpdate()
        {
            if (playerTransform == null)
                return;

            // Update the angle for circular motion
            angle += ORBITAL_SPEED * Time.fixedDeltaTime;

            // Calculate the new target position in the circular path
            float x = playerTransform.position.x + ORBITAL_RADIUS * Mathf.Cos(angle);
            float y = playerTransform.position.y + ORBITAL_RADIUS * Mathf.Sin(angle);

            // Add random offset to the target position
            x += Random.Range(-RANDOM_OFFSET, RANDOM_OFFSET);
            y += Random.Range(-RANDOM_OFFSET, RANDOM_OFFSET);

            targetPosition = new Vector2(x, y);

            // Smoothly move the ball to the new target position
            Vector2 newPosition = Vector2.Lerp(rb.position, targetPosition, SMOOTHING_FACTOR);
            rb.MovePosition(newPosition);
        }

        void OnDestroy()
        {
            Bodies.Remove(this);
        }

        private void UpdateTargetPosition()
        {
            float x = playerTransform.position.x + ORBITAL_RADIUS * Mathf.Cos(angle);
            float y = playerTransform.position.y + ORBITAL_RADIUS * Mathf.Sin(angle);
            x += Random.Range(-RANDOM_OFFSET, RANDOM_OFFSET);
            y += Random.Range(-RANDOM_OFFSET, RANDOM_OFFSET);
            targetPosition = new Vector2(x, y);
        }

        private void IgnoreCollisionWithPlayer(Rigidbody2D rb)
        {
            Collider2D playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), playerCollider);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<Enemy.EnemyHealth>(out Enemy.EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(damage);
                }
                else if (collision.TryGetComponent<Enemy.BossHealth>(out Enemy.BossHealth bossHealth))
                {
                    bossHealth.TakeDamage(damage);
                }
            }
            else if (collision.CompareTag("Border"))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
