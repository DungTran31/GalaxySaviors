using DungTran31.Core;
using DungTran31.GamePlay.Enemy.SO;
using DungTran31.UI;
using System;
using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private FloatingHealthBar floatingHealthBar;
        [SerializeField] private EnemyHealthSO enemyHealthSO;
        private float currentHealth;
        private bool isPoisoned = false;
        private bool isFrozen = false;

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public struct EnemyDeathEventArgs
        {
            public Vector3 Position;

            public EnemyDeathEventArgs(Vector3 position)
            {
                Position = position;
            }
        }

        public static event Action<EnemyDeathEventArgs> OnEnemyDeath;

        private void OnEnable()
        {
            currentHealth = enemyHealthSO.MaxHealth;
            floatingHealthBar = GetComponentInChildren<FloatingHealthBar>();
            floatingHealthBar.UpdateHealthBar(currentHealth, enemyHealthSO.MaxHealth);
        }

        public void TakeDamage(float amount)
        {
            ApplyDamage(amount);
        }

        public void TakeIceDamage(float amount)
        {
            if (!isFrozen) // Prevent stacking freeze effects
            {
                StartCoroutine(FreezeEnemy(enemyHealthSO.FreezeDuration));
            }
            ApplyDamage(amount);
        }

        public void TakePoisonDamage(float amount)
        {
            if (!isPoisoned) // Prevent stacking poison effects
            {
                StartCoroutine(PoisonEnemy(amount, enemyHealthSO.PoisonDuration));
            }
        }

        public void Die()
        {
            ApplyDamage(enemyHealthSO.MaxHealth);
        }

        private void ApplyDamage(float amount)
        {
            ShowDamage(amount.ToString());
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, enemyHealthSO.MaxHealth);
            floatingHealthBar.UpdateHealthBar(currentHealth, enemyHealthSO.MaxHealth);
            if (currentHealth <= 0)
            {
                Instantiate(enemyHealthSO.BloodEffect, transform.position, Quaternion.identity);
                Instantiate(enemyHealthSO.BloodSplash, transform.position, Quaternion.identity);
                OnEnemyDeath?.Invoke(new EnemyDeathEventArgs(transform.position));
                gameObject.SetActive(false);
            }
        }

        private IEnumerator FreezeEnemy(float duration)
        {
            isFrozen = true;
            // Initialize variables to store original speeds to restore later
            float originalSpeed = 0f;
            float originalRangedSpeed = 0f;

            // Attempt to get the movement components
            var movementComponent = GetComponent<EnemyAI>();
            var movementRangedComponent = GetComponent<RangedEnemyAI>();

            // Check if the components exist before trying to access their properties
            if (movementComponent != null)
            {
                originalSpeed = movementComponent.speed;
                movementComponent.speed = 0; // Disable movement
            }

            if (movementRangedComponent != null)
            {
                originalRangedSpeed = movementRangedComponent.speed;
                movementRangedComponent.speed = 0; // Disable movement
            }

            // Optionally, change the enemy's appearance to indicate it's frozen
            if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = Color.blue; // Change color to blue to indicate freezing
            }

            yield return new WaitForSeconds(duration);

            // Restore the enemy's state after the freeze duration
            if (movementComponent != null)
            {
                movementComponent.speed = originalSpeed; // Re-enable movement
            }

            if (movementRangedComponent != null)
            {
                movementRangedComponent.speed = originalRangedSpeed; // Re-enable movement
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white; // Restore the original color
            }
            isFrozen = false;
        }

        private IEnumerator PoisonEnemy(float damage, float duration)
        {
            isPoisoned = true;
            float elapsed = 0;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            while (elapsed < duration)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.green; // Change color to green to indicate poisoning
                }
                ApplyDamage(damage);
                yield return new WaitForSeconds(enemyHealthSO.PoisonTickTime);
                elapsed += enemyHealthSO.PoisonTickTime;
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white; // Restore the original color
                }
            }
            isPoisoned = false;
        }

        public void ShowDamage(string text)
        {
            if (enemyHealthSO.FloatingTextPrefab)
            {
                GameObject floatingText = Instantiate(enemyHealthSO.FloatingTextPrefab, transform.position, Quaternion.identity);
                floatingText.GetComponentInChildren<TextMesh>().text = text;
            }
        }
    }
}
