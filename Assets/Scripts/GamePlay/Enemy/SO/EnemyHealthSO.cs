using UnityEngine;

namespace DungTran31.GamePlay.Enemy.SO
{
    [CreateAssetMenu(fileName = "EnemyHealthSO", menuName = "ScriptableObjects/EnemyHealthSO", order = 2)]
    public class EnemyHealthSO : ScriptableObject
    {
        [field: Header("Health")]
        [field: SerializeField] public GameObject FloatingTextPrefab { get; private set; }
        [field: SerializeField] public GameObject BloodEffect { get; private set; }
        [field: SerializeField] public GameObject BloodSplash { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: Header("Effect")]
        [field: SerializeField] public float FreezeDuration { get; private set; }
        [field: SerializeField] public float PoisonTickTime { get; private set; }
        [field: SerializeField] public float PoisonDuration { get; private set; }
    }
}
