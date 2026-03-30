using UnityEngine;

namespace DungTran31.GamePlay.Player.SO
{
    [CreateAssetMenu(fileName = "BulletSO", menuName = "ScriptableObjects/PlayerBulletSO", order = 3)]
    public class PlayerBulletSO : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; } = 100f;
        [field: Range(1, 50)]
        [field: SerializeField] public float Speed { get; private set; } = 10f;
        [field: Range(1, 10)]
        [field: SerializeField] public float LifeTime { get; private set; } = 10f;

        private float originalDamage;
        private float originalSpeed;
        private float originalLifeTime;

        private void OnEnable()
        {
            originalDamage = Damage;
            originalSpeed = Speed;
            originalLifeTime = LifeTime;
        }

        public void IncreaseDamageByPercentage(float percentage)
        {
            Damage += Mathf.RoundToInt(originalDamage * (percentage / 100f));
        }

        public void ResetToOriginalValues()
        {
            Damage = originalDamage;
            Speed = originalSpeed;
            LifeTime = originalLifeTime;
        }
    }
}
