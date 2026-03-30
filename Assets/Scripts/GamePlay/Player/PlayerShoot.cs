using UnityEngine;
using DungTran31.Utilities;
using DungTran31.UI;
using DungTran31.Core;

namespace DungTran31.GamePlay.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        // Single fallback firing point (kept for backward compatibility)
        [SerializeField] private Transform firingPoint;
        // Support multiple firing points (e.g., level 5 -> 3 firing points)
        [SerializeField] private Transform[] firingPoints;

        [Range(0.1f, 2f)]
        [SerializeField] private float fireRate = 0.5f;

        private float fireTimer;
        // Define bullet types
        public enum BulletType { fire, poison, ice, black }
        private BulletType currentBulletType = BulletType.fire;
        private BulletTypeUI bulletTypeUI;

        private void Start()
        {
            // Avoid obsolete FindObjectOfType by using the newer API.
            // This returns the first instance found (like the previous behavior).
            bulletTypeUI = UnityEngine.Object.FindFirstObjectByType<BulletTypeUI>();
            UpdateBulletTypeUI();
        }

        private void Update()
        {
            if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return;

            if (Input.GetKeyDown(KeyCode.Space) && fireTimer <= 0f)
            {
                AudioManager.Instance.PlaySfx(AudioManager.Instance.shoot);
                Shoot();
                fireTimer = fireRate;
            }
            else
            {
                fireTimer -= Time.deltaTime;
            }

            // Switch bullet type forward
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchBulletType(true);
            }
            // Switch bullet type backward
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchBulletType(false);
            }
        }

        private void Shoot()
        {
            // Use the current bullet type to determine which bullet to shoot
            string poolTag = currentBulletType.ToString().ToLower() + "Bullet"; // Assuming your pool tags are named accordingly

            // Get the active firing transforms: prefer the array (for level 5 with 3 firing points),
            // otherwise fall back to the single firingPoint.
            var targets = GetActiveFiringTransforms();
            foreach (var fp in targets)
            {
                if (fp == null) continue;
                GameObject bullet = ObjectPooler.Instance.SpawnFromPool(poolTag, fp.position, fp.rotation);

                if (bullet != null)
                {
                    if (bullet.TryGetComponent<TrailRenderer>(out var trailRenderer))
                    {
                        // Ensure the TrailRenderer is properly reset or managed
                        trailRenderer.Clear();
                    }
                }
            }
        }

        private System.Collections.Generic.IEnumerable<Transform> GetActiveFiringTransforms()
        {
            if (firingPoints != null && firingPoints.Length > 0)
            {
                foreach (var t in firingPoints) yield return t;
                yield break;
            }

            if (firingPoint != null)
            {
                yield return firingPoint;
                yield break;
            }

            yield break;
        }

        private void SwitchBulletType(bool forward)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.switched);
            int bulletTypeCount = System.Enum.GetValues(typeof(BulletType)).Length;
            currentBulletType = (BulletType)(((int)currentBulletType + (forward ? 1 : -1) + bulletTypeCount) % bulletTypeCount);
            UpdateBulletTypeUI();
            Debug.Log("Switched to bullet type: " + currentBulletType);
        }
        private void UpdateBulletTypeUI()
        {
            if (bulletTypeUI != null)
            {
                bulletTypeUI.UpdateBulletTypeUI((int)currentBulletType);
            }
        }
    }
}
