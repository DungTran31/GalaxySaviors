using UnityEngine;

namespace DungTran31
{
    public class PlayerHub : MonoBehaviour
    {
        // Movement speed in units per second
        [SerializeField] private float moveSpeed = 5f;

        // Rotation speed in degrees per second (used to smoothly turn toward movement direction)
        [SerializeField] private float rotationSpeed = 720f;

        // If a Rigidbody is attached, movement will use Rigidbody.MovePosition for smooth physics integration.
        // Otherwise movement falls back to transform.Translate.
        [SerializeField] private bool useRigidbody = true;

        private Rigidbody rb;
        private Vector3 inputVector;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            // If no Rigidbody is present, disable the Rigidbody path to avoid null checks each FixedUpdate.
            if (rb == null)
            {
                useRigidbody = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Read input from both WASD and arrow keys (Unity's default "Horizontal" and "Vertical" axes).
            // Use GetAxisRaw for snappier input (no smoothing).
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Build a movement vector in world XY plane (for 2D style up/down) or XZ if you want forward/back.
            // Fix: map vertical input to Y so the player moves up/down as well as left/right.
            inputVector = new Vector3(horizontal, vertical, 0f);

            // Normalize so diagonal movement isn't faster than straight movement.
            if (inputVector.sqrMagnitude > 1f)
            {
                inputVector.Normalize();
            }

            // If not using Rigidbody, apply movement directly here (frame-rate dependent).
            if (!useRigidbody)
            {
                transform.Translate(inputVector * moveSpeed * Time.deltaTime, Space.World);
            }
        }

        // FixedUpdate is called at a fixed interval and is the preferred place to apply Rigidbody movement.
        void FixedUpdate()
        {
            if (useRigidbody && rb != null)
            {
                Vector3 newPosition = rb.position + inputVector * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);

                // Rotate to face movement direction (only when there's meaningful input)
                if (inputVector.sqrMagnitude > 0.0001f)
                {
                    float targetAngle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                    rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                }
            }
            else
            {
                // If not using Rigidbody, handle rotation here (frame-rate dependent)
                if (inputVector.sqrMagnitude > 0.0001f)
                {
                    float targetAngle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}