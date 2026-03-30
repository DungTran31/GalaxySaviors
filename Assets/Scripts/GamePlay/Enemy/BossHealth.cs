using DungTran31.Core;
using DungTran31.UI;
using System;
using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class BossHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int bossID;
        [SerializeField] private BossHealthBar bossHealthBar;
        [SerializeField] private GameObject floatingTextPrefab;
        [SerializeField] private GameObject bloodEffect;
        [SerializeField] private GameObject bloodSplash;
        [SerializeField] private float maxHealth = 200f;
        [Header("Status Effects")]
        [SerializeField] private float poisonDuration = 5f;
        [SerializeField] private float poisonTickTime = 1f;
        [SerializeField] private float freezeDuration = 3f;

        private float currentHealth;
        private bool isPoisoned = false;
        private bool isFrozen = false;

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public struct BossDeathEventArgs
        {
            public Vector3 Position;

            public BossDeathEventArgs(Vector3 position)
            {
                Position = position;
            }
        }

        public static event Action<BossDeathEventArgs> OnBossDeath;

        private void OnEnable()
        {
            currentHealth = maxHealth;
            bossHealthBar = GetComponentInChildren<BossHealthBar>();
            bossHealthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        public void TakeDamage(float amount)
        {
            ApplyDamage(amount);
        }

        public void TakeIceDamage(float amount)
        {
            if (!isFrozen) // Prevent stacking freeze effects
            {
                StartCoroutine(FreezeEnemy(freezeDuration));
            }
            ApplyDamage(amount);
        }

        public void TakePoisonDamage(float amount)
        {
            if (!isPoisoned) // Prevent stacking poison effects
            {
                StartCoroutine(PoisonEnemy(amount, poisonDuration));
            }
        }

        private void ApplyDamage(float amount)
        {
            ShowDamage(amount.ToString());
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            bossHealthBar.UpdateHealthBar(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                Instantiate(bloodEffect, transform.position, Quaternion.identity);
                Instantiate(bloodSplash, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySfx(AudioManager.Instance.enemyDeath);
                Debug.Log("Boss defeated: " + bossID);
                LevelManager.SetBossCompleted(bossID);
                OnBossDeath?.Invoke(new BossDeathEventArgs(transform.position));
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
                yield return new WaitForSeconds(poisonTickTime);
                elapsed += poisonTickTime;
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white; // Restore the original color
                }
            }
            isPoisoned = false;
        }

        public void ShowDamage(string text)
        {
            if (floatingTextPrefab)
            {
                GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                floatingText.GetComponentInChildren<TextMesh>().text = text;
            }
        }

        // Ví dụ test bằng phím
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                currentHealth = 0;
            }
        }
    }
}
