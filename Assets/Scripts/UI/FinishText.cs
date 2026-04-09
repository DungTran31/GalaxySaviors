using DG.Tweening;
using DungTran31.GamePlay.Enemy;
using TMPro;
using UnityEngine;

namespace DungTran31.UI
{
    public class FinishText : MonoBehaviour
    {
        [SerializeField] private float moveUpDistance = 2f;
        [SerializeField] private float duration = 1f;

        private Tween moveTween;
        private Tween fadeTween;

        private TMP_Text tmpText;

        private void Awake()
        {
            // Cache TMP component and disable only the text component (not the whole GameObject)
            if (!TryGetComponent(out tmpText))
            {
                Debug.LogWarning("TMP_Text component is missing on FinishText GameObject.");
                enabled = false;
                return;
            }

            tmpText.enabled = false;
        }

        private void OnEnable()
        {
            BossHealth.OnBossDeath += HandleBossDeath;
        }

        private void OnDisable()
        {
            BossHealth.OnBossDeath -= HandleBossDeath;
        }

        private void HandleBossDeath(BossHealth.BossDeathEventArgs args)
        {
            PlayEffect();
        }

        private void PlayEffect()
        {
            // Enable only the text rendering
            tmpText.enabled = true;

            // reset for re-play
            moveTween?.Kill();
            fadeTween?.Kill();

            var c = tmpText.color;
            c.a = 1f;
            tmpText.color = c;

            // Move the GameObject up
            moveTween = transform.DOMoveY(transform.position.y + moveUpDistance, duration)
                .SetEase(Ease.OutQuad);

            // Fade out TextMeshPro alpha, then disable the TMP component
            fadeTween = tmpText.DOFade(0f, duration)
                .OnComplete(() => tmpText.enabled = false);
        }
    }
}