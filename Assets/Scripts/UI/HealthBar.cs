using UnityEngine;
using UnityEngine.UI;
using DungTran31.GamePlay.Player;

namespace DungTran31.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image currentHealthBar;
        [SerializeField] private PlayerHealth playerHealth;

        private void Update()
        {
            currentHealthBar.fillAmount = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        }
    }
}
