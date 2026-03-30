using UnityEngine;

namespace DungTran31
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private float _speed;

        private void Update() => transform.Rotate(_speed * Time.deltaTime * _rotation);
    }
}
