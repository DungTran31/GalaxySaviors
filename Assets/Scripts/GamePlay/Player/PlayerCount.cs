using DungTran31.Core;
using DungTran31.GamePlay.Enemy;
using TMPro;
using UnityEngine;

namespace DungTran31.GamePlay.Player
{
    public class PlayerCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI killRemainingCountText;
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private int targetKillCount;

        public int KillCount { get; private set; }
        private bool bossSpawned = false;

        private void Start()
        {
            bossSpawned = false;
            KillCount = 0;
            UpdateKillCountUI(); // Initialize UI on start
        }

        private void OnEnable() => EnemyHealth.OnEnemyDeath += IncrementKillCount;

        private void OnDisable() => EnemyHealth.OnEnemyDeath -= IncrementKillCount;

        private void IncrementKillCount(EnemyHealth.EnemyDeathEventArgs args)
        {
            if (KillCount < targetKillCount)
            {
                KillCount++; // Increment the kill count
                UpdateKillCountUI(); // Update the UI
            }
        }

        // Updates the kill count UI text
        private void UpdateKillCountUI()
        {
            if (killRemainingCountText != null)
            {
                int remainingKillCount = targetKillCount - KillCount;

                if (remainingKillCount <= 0)
                {
                    if (!bossSpawned)
                    {
                        // Spawn the boss when the player reaches the target kill count
                        Instantiate(bossPrefab, Vector3.zero, Quaternion.identity);
                        AudioManager.Instance.ChangeMusic(AudioManager.Instance.bossBackground);
                        bossSpawned = true;
                    }
                }
                killRemainingCountText.text = remainingKillCount.ToString();
            }
        }
    }
}
