using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class BossMovement : MonoBehaviour
    {
        [SerializeField] internal float speed = 5f;
        [SerializeField] private float playerAwarenessDistance = 10f;

        private Rigidbody2D rb;
        private Transform target;
        public Vector2 direction;
        public Vector2 targetDirection;
        private float changeDirectionCooldown;
        private bool awareOfPlayer;
        private BossAttack bossAttack;

        private void Awake()
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            bossAttack = GetComponent<BossAttack>();
        }

        private void Update()
        {
            if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return;
            if (target != null)
                AwarePlayerDistance();
            PreventEnemyGoingOffScreen();
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

        private void FixedUpdate()
        {
            if (bossAttack != null && (bossAttack.isDashing || bossAttack.isReturning))
            {
                rb.linearVelocity = Vector2.zero;
                return;
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
    }
}
