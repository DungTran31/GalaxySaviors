using DungTran31.Core;
using System;
using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private GameObject deathEffect;
        [SerializeField] private GameObject bleedEffect;
        [field: SerializeField] public float MaxHealth { get; private set; } = 400f;
        public float CurrentHealth { get; private set; }

        [Header("iFrames")]
        [SerializeField] private float iFrameDuration = 2f;
        [SerializeField] private int numberOfFlashes = 3;
        [SerializeField] private bool isInvulnerable = false;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GameObject.FindGameObjectWithTag("PlayerHead").GetComponent<SpriteRenderer>();
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (isInvulnerable) return;
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
            Instantiate(bleedEffect, transform.position, Quaternion.identity);
            if (CurrentHealth <= 0)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySfx(AudioManager.Instance.death);
                Destroy(gameObject);
                Time.timeScale = 0;
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ChangeGameOverScreenState(true);
                }
                else
                {
                    Debug.LogError("UIManager.Instance is null. Ensure UIManager is properly initialized.");
                }
            }
            else
            {
                StartCoroutine(Invunerability());
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public void IncreaseMaxHealthByPercentage(float percentage)
        {
            MaxHealth += Mathf.RoundToInt(MaxHealth * (percentage / 100f));
            CurrentHealth = MaxHealth; // optional: heal the player to full health
        }

        public void ApplySlow(float slowPercentage, float duration)
        {
            StartCoroutine(SlowEffect(slowPercentage, duration));
        }

        private IEnumerator SlowEffect(float slowPercentage, float duration)
        {
            // Assuming the player has a movement script with a speed variable
            if (TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                float originalSpeed = playerMovement.moveSpeed;
                playerMovement.moveSpeed *= (1 - slowPercentage / 100f);
                yield return new WaitForSeconds(duration);
                playerMovement.moveSpeed = originalSpeed;
            }
        }

        private IEnumerator Invunerability()
        {
            // Ignore collisions with enemies
            Physics2D.IgnoreLayerCollision(gameObject.layer, 10, true);
            // Ignore collisions with bullets
            Physics2D.IgnoreLayerCollision(gameObject.layer, 11, true);
            isInvulnerable = true;

            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
                yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
                spriteRenderer.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            }

            // Re-enable collisions with enemies
            Physics2D.IgnoreLayerCollision(gameObject.layer, 10, false);
            // Re-enable collisions with bullets
            Physics2D.IgnoreLayerCollision(gameObject.layer, 11, false);
            isInvulnerable = false;
        }
    }
}
