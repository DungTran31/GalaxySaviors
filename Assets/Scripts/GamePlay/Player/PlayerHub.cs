using UnityEngine;

namespace DungTran31.GamePlay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerHub : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSmoothTime = 0.12f;

        private Rigidbody2D rb;

        // Input
        private Vector2 _input;

        // Rotation
        private float _turnSmoothVelocity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");

            if (_input.sqrMagnitude > 1f)
                _input.Normalize();
        }

        private void FixedUpdate()
        {
            HandleMovement2D();
        }

        private void HandleMovement2D()
        {
            rb.linearVelocity = _input * moveSpeed;

            if (_input.sqrMagnitude <= 0.0001f)
                return;

            float targetAngle = Mathf.Atan2(_input.y, _input.x) * Mathf.Rad2Deg;

            float smoothAngle = Mathf.SmoothDampAngle(
                rb.rotation,
                targetAngle,
                ref _turnSmoothVelocity,
                rotationSmoothTime
            );

            rb.MoveRotation(smoothAngle);
        }
    }
}