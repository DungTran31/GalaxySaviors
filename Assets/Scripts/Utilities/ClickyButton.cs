using DungTran31.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DungTran31.Utilities
{
    public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _img;
        [SerializeField] private Sprite _default, _pressed;
        public void OnPointerDown(PointerEventData eventData)
        {
            _img.sprite = _pressed;
            AudioManager.Instance.PlaySfx(AudioManager.Instance.pressed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _img.sprite = _default;
        }
    }
}