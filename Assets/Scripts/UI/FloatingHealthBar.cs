using UnityEngine;
using UnityEngine.UI;

namespace DungTran31.UI
{
    public class FloatingHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        private Camera MainCamera;
        private void Start()
        {
            MainCamera = Camera.main;
        }

        public void UpdateHealthBar(float currentValue, float maxValue)
        {
            slider.value = currentValue / maxValue;
        }

        private void Update()
        {
            if (target != null)
            {
                transform.SetPositionAndRotation(target.position + offset, MainCamera.transform.rotation);
            }
        }
    }
}
