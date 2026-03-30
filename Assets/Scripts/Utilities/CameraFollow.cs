using UnityEngine;

namespace DungTran31.Utilities 
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private float damping;

        public Transform target;

        private Vector3 _vel = Vector3.zero;

        private void FixedUpdate()
        {
            if (!target) return;

            Vector3 targetPosition = target.position + offset;
            targetPosition.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _vel, damping);
        }
    }
}