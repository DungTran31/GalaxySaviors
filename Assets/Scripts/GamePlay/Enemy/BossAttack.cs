using UnityEngine;
using DG.Tweening;

namespace DungTran31.GamePlay.Enemy
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeField] private float damage = 100f;
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float attackCooldown = 2f;

        private Transform target;
        private Vector2 attackStartPosition;
        internal bool isDashing = false;
        internal bool isReturning = false;
        private bool hasCollided = false;
        private float cooldownTimer = 0f;

        private void Start()
        {
            GetTarget();
        }

        private void Update()
        {
            if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return;

            if (!target)
            {
                GetTarget();
                hasCollided = false; // Reset the flag when a new target is acquired
            }

            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
                return;
            }

            Attack();
        }

        private void Attack()
        {
            if (isDashing)
            {
                // DashTowardsPlayer is now handled by DOTween
            }
            else if (isReturning)
            {
                // ReturnToAttackStartPosition is now handled by DOTween
            }
            else if (target && Vector2.Distance(transform.position, target.position) <= dashDistance)
            {
                StartDash();
            }
        }

        private void GetTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        private void StartDash()
        {
            if (target == null) return; // Ensure target is not null before starting the dash

            isDashing = true;
            attackStartPosition = transform.position; // Store the position where the boss starts the attack
            DashTowardsPlayer();
        }

        private void DashTowardsPlayer()
        {
            transform.DOMove(target.position, dashSpeed).SetSpeedBased(true).OnComplete(() =>
            {
                isDashing = false;
                isReturning = true;
                cooldownTimer = attackCooldown;
                ReturnToAttackStartPosition();
            });
        }

        private void ReturnToAttackStartPosition()
        {
            transform.DOMove(attackStartPosition, dashSpeed).SetSpeedBased(true).OnComplete(() =>
            {
                isReturning = false;
                hasCollided = false; // Reset the collision flag when the boss returns to the start position
            });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !hasCollided)
            {
                collision.gameObject.GetComponent<Player.PlayerHealth>().TakeDamage(damage);
                hasCollided = true; // Set the flag to true after processing the collision
            }
        }
    }
}
