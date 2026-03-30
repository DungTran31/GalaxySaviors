
namespace DungTran31.Core
{
    using UnityEngine;

    public class EndlessBackground : MonoBehaviour
    {
        [SerializeField] private float backgroundSize;
        [SerializeField] private float parallaxSpeed;

        private Transform _cameraTransform;
        private Transform[] _layers;
        private const float ViewZone = 10;
        private int _leftIndex;
        private int _rightIndex;

        private void Start()
        {
            if (Camera.main != null) _cameraTransform = Camera.main.transform;
            _layers = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                _layers[i] = transform.GetChild(i);

            _leftIndex = 0;
            _rightIndex = _layers.Length - 1;
        }

        private void Update()
        {
            float deltaX = _cameraTransform.position.x * parallaxSpeed;
            transform.position = new Vector3(deltaX, transform.position.y, transform.position.z);

            if (_cameraTransform.position.x < (_layers[_leftIndex].transform.position.x + ViewZone))
                ScrollLeft();

            if (_cameraTransform.position.x > (_layers[_rightIndex].transform.position.x - ViewZone))
                ScrollRight();
        }

        private void ScrollLeft()
        {
            _layers[_rightIndex].position = new Vector3((_layers[_leftIndex].position.x - backgroundSize), _layers[_rightIndex].position.y, _layers[_rightIndex].position.z);
            _leftIndex = _rightIndex;
            _rightIndex--;
            if (_rightIndex < 0)
                _rightIndex = _layers.Length - 1;
        }

        private void ScrollRight()
        {
            _layers[_leftIndex].position = new Vector3((_layers[_rightIndex].position.x + backgroundSize), _layers[_leftIndex].position.y, _layers[_leftIndex].position.z);
            _rightIndex = _leftIndex;
            _leftIndex++;
            if (_leftIndex == _layers.Length)
                _leftIndex = 0;
        }
    }
}
