using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungTran31.Core
{
    public static class LevelManager
    {
        public static event Action<int> OnBossUnlocked;

        // ===================== CHECK =====================

        public static bool IsBossCompleted(int id)
        {
            return PlayerPrefs.GetInt($"Boss_{id}_Completed", 0) == 1;
        }

        public static bool IsBossUnlocked(int id)
        {
            // Boss 1 luôn mở
            if (id == 1) return true;

            return PlayerPrefs.GetInt($"Boss_{id}_Unlocked", 0) == 1;
        }

        // ===================== SET =====================

        public static void SetBossCompleted(int id)
        {
            PlayerPrefs.SetInt($"Boss_{id}_Completed", 1);
            Debug.Log($"Boss {id} Completed");

            CheckUnlocks();
        }

        private static void UnlockBoss(int id)
        {
            if (!IsBossUnlocked(id))
            {
                PlayerPrefs.SetInt($"Boss_{id}_Unlocked", 1);
                Debug.Log($"Boss {id} Unlocked");

                OnBossUnlocked?.Invoke(id);
            }
        }

        // ===================== LOGIC =====================

        private static void CheckUnlocks()
        {
            // Boss 1 → mở 2,3,4
            if (IsBossCompleted(1))
            {
                UnlockBoss(2);
                UnlockBoss(3);
                UnlockBoss(4);
            }

            // Đếm boss 2,3,4
            int count = 0;
            if (IsBossCompleted(2)) count++;
            if (IsBossCompleted(3)) count++;
            if (IsBossCompleted(4)) count++;

            // >=2 → mở boss 5
            if (count >= 2)
            {
                UnlockBoss(5);
            }

            PlayerPrefs.Save();
        }

        // ===================== DEBUG =====================

        public static void ResetAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Reset All Progress");
        }

    }
}
