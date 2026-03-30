using UnityEngine;
using DungTran31.Utilities;
using DungTran31.Core;

namespace DungTran31.GamePlay.Enemy
{
    public class RangedEnemyAI : MonoBehaviour
    {
        // RangedEnemyAttack variables
        [SerializeField] private GameObject deathEffect;
        [SerializeField] private Transform firingPoint;
        [SerializeField] private float distanceToShoot = 10f;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float _orbitRadius = 5f;

        private Transform target;
        private float timeToFire;
        private bool hasCollided = false; // Add a flag to check if collision has been processed to ensure only happens once

        // RangedEnemyMovement variables
        [SerializeField] internal float speed = 5f;
        [SerializeField] private float playerAwarenessDistance = 10f;
        [SerializeField] private float distanceToStop = 5f;

        private Rigidbody2D rb;
        private Vector2 direction;
        private Vector2 targetDirection;
        private float changeDirectionCooldown;
        private bool awareOfPlayer;

        private void Start()
        {
            GetTarget();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (UIManager.IsGamePaused) return;

            if (!target)
            {
                GetTarget();
                hasCollided = false; // Reset the flag when a new target is acquired
            }
            else
            {
                RotateFiringPoint();
                if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return; // Example check from RangedEnemyMovement
                AwarePlayerDistance(); // Example method call from RangedEnemyMovement
                PreventEnemyGoingOffScreen(); // Example method call from RangedEnemyMovement
            }
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                UpdateTargetDirection(); // Example method call from RangedEnemyMovement
                SetVelocity(); // Example method call from RangedEnemyMovement

                if (Vector2.Distance(transform.position, target.position) <= distanceToShoot)
                {
                    Shoot(); // Example method call from RangedEnemyAttack
                }
            }
        }

        private void Shoot()
        {
            if (timeToFire <= 0)
            {
                ObjectPooler.Instance.SpawnFromPool("enemyBullet", firingPoint.position, firingPoint.rotation);
                timeToFire = fireRate;
            }
            else
            {
                timeToFire -= Time.deltaTime;
            }
        }

        private void RotateFiringPoint()
        {
            Vector2 targetDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            firingPoint.rotation = Quaternion.Euler(0, 0, angle);
            Vector2 orbitPosition = (Vector2)transform.position + (targetDirection.normalized * _orbitRadius);
            firingPoint.position = orbitPosition;
        }

        private void GetTarget()
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !hasCollided)
            {
                if (deathEffect != null) // Check if deathEffect is assigned
                {
                    Instantiate(deathEffect, target.position, Quaternion.identity);
                }
                Destroy(collision.gameObject);

                target = null;
                hasCollided = true; // Set the flag to true after processing the collision
            }
        }

        private void AwarePlayerDistance()
        {
            Vector2 directionTemp = (target.position - transform.position);
            direction = directionTemp.normalized;

            if (directionTemp.magnitude <= playerAwarenessDistance)
            {
                awareOfPlayer = true;
            }
            else
            {
                awareOfPlayer = false;
            }
        }

        private void SetVelocity()
        {
            if (target != null) // Check if the target exists
            {
                float distance = Vector2.Distance(transform.position, target.position);
                if (distance >= distanceToStop)
                {
                    rb.linearVelocity = targetDirection * speed;
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
            else // If the target doesn't exist, stop moving
            {
                rb.linearVelocity = Vector2.zero;
            }
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

        private void PreventEnemyGoingOffScreen()
        {
            float minX = -48f; // Minimum X coordinate
            float maxX = 48f;  // Maximum X coordinate
            float minY = -34.5f; // Minimum Y coordinate
            float maxY = 34.5f;  // Maximum Y coordinate

            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

            transform.position = new Vector2(clampedX, clampedY);
        }
    }
}

