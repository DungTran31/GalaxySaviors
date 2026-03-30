using UnityEngine;

namespace DungTran31.Utilities
{
    public class ParallaxEffect : MonoBehaviour
    {
        public Camera cam;
        public Transform followTarget;

        // Starting position for the parallax game object
        private Vector2 _startingPosition;

        // Start Z value of the parallax game object
        private float _startingZ;

        // Distance that the came ra has moved from the starting position of the parallax object
        private Vector2 CamMoveSinceStart => (Vector2)cam.transform.position - _startingPosition;

        private float ZDistanceFromTarget => transform.position.z - followTarget.position.z;

        // If object is in front of target , use near clip plane . if behind target , user far clip plane
        private float ClippingPlane => (cam.transform.position.z + (ZDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

        // The futher the object from the player , use faster the ParallaxEffect object will move . Drag it's Z value closer to the target to make it move slower
        private float ParallaxFactor => Mathf.Abs(ZDistanceFromTarget) / ClippingPlane;



        private void Start()
        {
            _startingPosition = transform.position;
            _startingZ = transform.position.z;  
        }

        private void Update()
        {
            // When the target moves , move the parallax object the same distance times a multiplier
            Vector2 newPosition = _startingPosition + CamMoveSinceStart * ParallaxFactor;

            // The X/Y position changes based on target travel speed times the parallax factor , but z stay consistent  
            transform.position = new Vector3(newPosition.x, newPosition.y, _startingZ);
        }
    }
}