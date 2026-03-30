using UnityEngine;

namespace DungTran31.Core
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRend;
        [SerializeField] private int length;
        
        [SerializeField] private Transform targetDir;
        [SerializeField] private float targetDist;
        [SerializeField] private float smoothSpeed;
        [SerializeField] private float trailSpeed;
        
        [SerializeField] private Transform wiggleDir;
        [SerializeField] private float wiggleSpeed;
        [SerializeField] private float wiggleMagnitude;
        
        private Vector3[] _segmentPoses;
        private Vector3[] _segmentV;
        
        private void Start()
        {
            lineRend.positionCount = length;
            _segmentPoses = new Vector3[length];
            _segmentV = new Vector3[length];
        }

        private void Update()
        {
            wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
            
            _segmentPoses[0] = targetDir.position;

            for (int i = 1; i < _segmentPoses.Length; i++)
            {
                _segmentPoses[i] = Vector3.SmoothDamp(_segmentPoses[i], _segmentPoses[i - 1] - targetDir.right * targetDist, ref _segmentV[i], smoothSpeed + i / trailSpeed);
            }
            
            lineRend.SetPositions(_segmentPoses);
        }
    }
}
