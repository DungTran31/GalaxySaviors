using DungTran31.GamePlay.Player.SO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungTran31.GamePlay.Player
{
    public class PlayerExp : MonoBehaviour
    {
        [SerializeField] private GameObject levelUpEffect;
        [SerializeField] private Image experienceBar; 
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text experienceText;
        [SerializeField] private LevelSO levelConfig; 
        [SerializeField] private PlayerHealth playerHealth; // Reference to the PlayerHealth script
        [SerializeField] private PlayerMovement playerMovement; // Reference to the PlayerMovement script
        [SerializeField] private PlayerBulletSO fireBulletSO; // Reference to the FireBullet ScriptableObject
        [SerializeField] private PlayerBulletSO iceBulletSO; // Reference to the IceBullet ScriptableObject
        [SerializeField] private PlayerBulletSO blackBulletSO; // Reference to the BlackBullet ScriptableObject
        [SerializeField] private PlayerBulletSO poisonBulletSO; // Reference to the BlackBullet ScriptableObject
        [SerializeField] private int level = 1; // Start at level 1
        [SerializeField] private int experience = 0; // Start with 0 experience
        private int requireExperience;

        public void IncreaseExp(int value)
        {
            experience += value;
            if (experience >= requireExperience)
            {
                while (experience >= requireExperience)
                {
                    experience -= requireExperience;
                    LevelUp();
                }
            }
            UpdateUI();
        }

        public void LevelUp()
        {
            level++;
            Instantiate(levelUpEffect, transform.position, Quaternion.identity);
            CalculateRequireExp();

            // Assuming you have references set up
            playerHealth.IncreaseMaxHealthByPercentage(10); // Increase max health by 10%
            playerMovement.IncreaseMoveSpeedByPercentage(5); // Increase move speed by 5%

            List<int> percentageIncreases = new() { 5, 10, 15, 20 };

            // Shuffle the list to randomize the order of percentage increases
            System.Random rng = new();
            int n = percentageIncreases.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (percentageIncreases[k], percentageIncreases[n]) = (percentageIncreases[n], percentageIncreases[k]); // Using tuple to swap values
            }

            // Assign unique random percentage increases to each bullet type
            fireBulletSO.IncreaseDamageByPercentage(percentageIncreases[0]);
            iceBulletSO.IncreaseDamageByPercentage(percentageIncreases[1]);
            blackBulletSO.IncreaseDamageByPercentage(percentageIncreases[2]);
            poisonBulletSO.IncreaseDamageByPercentage(percentageIncreases[3]);
        }

        private void Start()
        {
            CalculateRequireExp();
            UpdateUI();
        }

        private void CalculateRequireExp()
        {
            requireExperience = levelConfig.GetRequiredExp(level);
            UpdateUI();
        }

        private void UpdateUI()
        {
            experienceBar.fillAmount = (float)experience / (float)requireExperience; 
            levelText.text = "Level: " + level;
            experienceText.text = experience + " / " + requireExperience + " Exp"; 
        }
    }
}
