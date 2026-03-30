using DungTran31.Core;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        private enum EnemyState
        {
            Roaming,
            Chase,
            Attack
        }

        [SerializeField] internal float speed = 5f;
        [SerializeField] private float playerAwarenessDistance = 10f;
        [SerializeField] private float damage = 100f;

        private Rigidbody2D rb;
        private Transform target;
        public Vector2 direction;
        public Vector2 targetDirection;
        private float changeDirectionCooldown;
        private bool awareOfPlayer;
        private EnemyState state;
        private bool hasCollided = false; // Add a flag to check if collision has been processed to ensure only happens once

        private void Awake()
        {
            state = EnemyState.Roaming;
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (UIManager.IsGamePaused) return;
            if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return;
            if (target != null)
                AwarePlayerDistance();
            PreventEnemyGoingOffScreen();
        }

        private void AwarePlayerDistance()
        {
            if (target == null) return; // Add null check

            Vector2 directionTemp = (target.position - transform.position);
            direction = directionTemp.normalized;

            if (directionTemp.magnitude <= playerAwarenessDistance)
            {
                awareOfPlayer = true;
                state = EnemyState.Chase; // Transition to Chase state when player is within awareness distance
            }
            else
            {
                awareOfPlayer = false;
                state = EnemyState.Roaming; // Transition to Roaming state when player is out of awareness distance
            }
        }

        private void FixedUpdate()
        {
            switch (state)
            {
                case EnemyState.Roaming:
                    Roaming();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }
            UpdateTargetDirection();
            SetVelocity();
        }

        private void SetVelocity()
        {
            rb.linearVelocity = targetDirection * speed;
        }

        private void UpdateTargetDirection()
        {
            if (awareOfPlayer)
            {
                targetDirection = direction;
            }
            else
            {
                HandleRandomDirectionChange();
            }
        }

        private void HandleRandomDirectionChange()
        {
            changeDirectionCooldown -= Time.deltaTime;
            if (changeDirectionCooldown <= 0)
            {
                targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                changeDirectionCooldown = Random.Range(1f, 5f);
            }
        }

        private void Roaming()
        {
            // Roaming behavior implementation
            HandleRandomDirectionChange();
        }

        private void Chase()
        {
            if (target == null) return; // Add null check

            // Chase behavior implementation
            if (Vector2.Distance(transform.position, target.position) <= 1f) // Example attack range
            {
                state = EnemyState.Attack; // Transition to Attack state when close enough to the player
            }
        }

        private void Attack()
        {
            // Attack behavior implementation
            // This could involve stopping movement and triggering an attack animation or logic
            rb.linearVelocity = Vector2.zero; // Stop movement during attack
        }

        private void PreventEnemyGoingOffScreen()
        {
            // Define the boundaries of the rectangle
            float minX = -48f; // Minimum X coordinate
            float maxX = 48f;  // Maximum X coordinate
            float minY = -34.5f; // Minimum Y coordinate
            float maxY = 34.5f;  // Maximum Y coordinate

            // Clamp the player's position within the defined boundaries
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

            // Update the player's position to the clamped position
            transform.position = new Vector2(clampedX, clampedY);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !hasCollided)
            {
                if (collision.TryGetComponent<Player.PlayerHealth>(out var playerHealth))
                {
                    playerHealth.TakeDamage(damage);
                }
                target = null;
                hasCollided = true; // Set the flag to true after processing the collision
                state = EnemyState.Roaming; // Transition back to Roaming state after attack
            }
        }
    }
}
