using UnityEngine;
using DG.Tweening;

namespace DungTran31
{
    public class LevelUpEffect : MonoBehaviour
    {
        [SerializeField] private float moveUpDistance = 2f;
        [SerializeField] private float duration = 1f;

        private void Start()
        {
            // Move the GameObject up
            transform.DOMoveY(transform.position.y + moveUpDistance, duration).SetEase(Ease.OutQuad);

            // Fade out the alpha of the SpriteRenderer
            if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.DOFade(0, duration).OnComplete(() => Destroy(gameObject));
            }
            else
            {
                Debug.LogWarning("SpriteRenderer component is missing on LevelUpEffect GameObject.");
            }
        }
    }
}
