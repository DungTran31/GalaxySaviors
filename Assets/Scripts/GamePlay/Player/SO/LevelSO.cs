using UnityEngine;

namespace DungTran31.GamePlay.Player.SO
{
    [CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/LevelSO", order = 4)]
    public class LevelSO : ScriptableObject
    {
        [Header("Animation Curve")]
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private int maxLevel;
        [SerializeField] private int maxRequiredExp;

        public int GetRequiredExp(int level)
        {
            return Mathf.RoundToInt(animationCurve.Evaluate(Mathf.InverseLerp(0, maxLevel, level)) * maxRequiredExp);
        }

    }
}
