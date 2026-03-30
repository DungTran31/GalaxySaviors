using DungTran31.GamePlay.Player;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyBlackHole : MonoBehaviour
    {
        [Header("Damage Settings")]
        public float damageAmount = 10f;

        [Header("Life Time")]
        [Tooltip("Time in seconds before this black hole is destroyed")]
        public float lifeTime = 5f;

        private void Start()
        {
            // Destroy this black hole after lifeTime seconds (default 5s)
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Try to get the player health / damage receiver component on the collided object
            var playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                // If the collider is a child (e.g. hitbox), check the root object as well
                playerHealth = collision.GetComponentInParent<PlayerHealth>();
            }

            // If we found a player health component, apply damage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
