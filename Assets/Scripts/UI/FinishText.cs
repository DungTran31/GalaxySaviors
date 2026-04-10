using DG.Tweening;
using DungTran31.Core;
using TMPro;
using UnityEngine;

namespace DungTran31.UI
{
    public class FinishText : MonoBehaviour
    {
        [SerializeField] private float moveUpDistance = 2f;
        [SerializeField] private float duration = 1f;

        [Header("Text")]
        [SerializeField] private string bossUnlockedTextFormat = "Boss {0} Unlocked!";

        private Tween moveTween;
        private Tween fadeTween;

        private TMP_Text tmpText;
        private RectTransform rectTransform;
        private Vector2 initialAnchoredPosition;
        private Vector3 initialLocalPosition;
        private string initialText;

        private void Awake()
        {
            // Cache TMP component and disable only the text component (not the whole GameObject)
            if (!TryGetComponent(out tmpText))
            {
                Debug.LogWarning("TMP_Text component is missing on FinishText GameObject.");
                enabled = false;
                return;
            }

            rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                initialAnchoredPosition = rectTransform.anchoredPosition;
            }

            initialLocalPosition = transform.localPosition;
            initialText = tmpText.text;

            tmpText.enabled = false;
        }

        private void OnEnable()
        {
            LevelManager.OnBossUnlocked += HandleBossUnlocked;
        }

        private void OnDisable()
        {
            LevelManager.OnBossUnlocked -= HandleBossUnlocked;
        }

        private void HandleBossUnlocked(int bossId)
        {
            var message = string.Format(bossUnlockedTextFormat, bossId);
            PlayEffect(message);
        }

        private void PlayEffect(string message)
        {
            tmpText.enabled = true;

            // reset for re-play
            moveTween?.Kill();
            fadeTween?.Kill();

            // Reset to a stable baseline, then animate in LOCAL/UI space (prevents moving too far/fast in world units)
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = initialAnchoredPosition;
            }
            else
            {
                transform.localPosition = initialLocalPosition;
            }

            tmpText.text = message;

            var c = tmpText.color;
            c.a = 1f;
            tmpText.color = c;

            // Move up (UI: anchoredPosition, World objects: localPosition)
            if (rectTransform != null)
            {
                moveTween = rectTransform
                    .DOAnchorPosY(initialAnchoredPosition.y + moveUpDistance, duration)
                    .SetEase(Ease.OutQuad);
            }
            else
            {
                moveTween = transform
                    .DOLocalMoveY(initialLocalPosition.y + moveUpDistance, duration)
                    .SetEase(Ease.OutQuad);
            }

            // Fade out TextMeshPro alpha, then disable the TMP component and restore original text
            fadeTween = tmpText
                .DOFade(0f, duration)
                .OnComplete(() =>
                {
                    tmpText.enabled = false;
                    tmpText.text = initialText;
                });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayEffect("Test Boss Unlocked!");
            }
        }
    }
}