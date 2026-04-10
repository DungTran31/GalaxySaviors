using DG.Tweening;
using DungTran31.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungTran31.UI
{
    public class FinishText : MonoBehaviour
    {
        [SerializeField] private float moveUpDistance = 2f;
        [SerializeField] private float duration = 1f;

        [Header("Delay")]
        [SerializeField] private float delayBetweenMessages = 1f;

        [Header("Text")]
        [SerializeField] private string bossUnlockedTextFormat = "Boss {0} Unlocked!";

        private Tween moveTween;
        private Tween fadeTween;

        private TMP_Text tmpText;
        private RectTransform rectTransform;
        private Vector2 initialAnchoredPosition;
        private Vector3 initialLocalPosition;
        private string initialText;

        private Queue<string> messageQueue = new Queue<string>();
        private bool isProcessing = false;

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

            messageQueue.Enqueue(message);

            if (!isProcessing)
            {
                StartCoroutine(ProcessQueue());
            }
        }

        // ===================== QUEUE =====================

        private IEnumerator ProcessQueue()
        {
            isProcessing = true;

            while (messageQueue.Count > 0)
            {
                yield return new WaitForSeconds(delayBetweenMessages);

                string message = messageQueue.Dequeue();

                PlayEffect(message);

                // đợi animation chạy xong
                yield return new WaitForSeconds(duration);
            }

            isProcessing = false;
        }

        // ===================== EFFECT =====================

        private void PlayEffect(string message)
        {
            tmpText.enabled = true;

            moveTween?.Kill();
            fadeTween?.Kill();

            // reset vị trí
            if (rectTransform != null)
                rectTransform.anchoredPosition = initialAnchoredPosition;
            else
                transform.localPosition = initialLocalPosition;

            tmpText.text = message;

            var c = tmpText.color;
            c.a = 1f;
            tmpText.color = c;

            // move
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

            // fade
            fadeTween = tmpText
                .DOFade(0f, duration)
                .OnComplete(() =>
                {
                    tmpText.enabled = false;
                    tmpText.text = initialText;
                });
        }

        // ===================== TEST =====================

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        HandleBossUnlocked(Random.Range(1, 6));
        //    }
        //}
    }
}