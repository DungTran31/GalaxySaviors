using DungTran31.Core;
using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private GameObject dashEffect;
        [SerializeField] private float rotationSpeed = 25f;
        [SerializeField] internal float moveSpeed = 10f;
        [SerializeField] private float dashSpeed = 50f;
        [SerializeField] private float startDashTime = 0.1f;
        [SerializeField] private float dashCooldown = 1f; 

        private Vector2 _direction;
        private Vector2 _cursorPos; 
        private Camera _camera;
        private Rigidbody2D _rb;
        private float dashTime;
        private float dashCooldownTimer = 0;
        private float originalMoveSpeed; // To store the original move speed
        private int dashDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
            originalMoveSpeed = moveSpeed; // Store the original move speed
        }

        private void Update()
        {
            if (UIManager.IsGamePaused) return;
            if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return;

            SetPlayerVelocity();
            //PreventPlayerGoingOffScreen();
            _cursorPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;
            }
            DashMovement();
        }

        private void DashMovement()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySfx(AudioManager.Instance.dash);
                dashDirection = 1;
                dashTime = startDashTime; // Reset dashTime
                dashCooldownTimer = dashCooldown; // Reset the cooldown timer
            }

            if (dashDirection == 1)
            {
                if (dashTime > 0)
                {
                    _rb.linearVelocity = transform.right * dashSpeed;
                    if (trailRenderer != null)
                    {
                        // Safe to access trailRenderer here
                        trailRenderer.time = 2.0f;
                        trailRenderer.emitting = true; // Enable the trail renderer
                    }
                    dashTime -= Time.deltaTime;
                }
                else
                {
                    dashDirection = 0;
                    if (trailRenderer != null)
                    {
                        trailRenderer.emitting = false; // Disable the trail renderer
                    }
                    _rb.linearVelocity = Vector2.zero; // Reset velocity after dash
                }
            }
        }


        private void SetPlayerVelocity()
        {
            // Only update direction and rotation if the mouse is moving
            if (_cursorPos != (Vector2)transform.position)
            {
                _direction = _cursorPos - (Vector2)transform.position;
                float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }

            // Always move towards the cursor position
            transform.position = Vector2.MoveTowards(transform.position, _cursorPos, moveSpeed * Time.deltaTime);
        }
        public void IncreaseMoveSpeedByPercentage(float percentage)
        {
            moveSpeed += Mathf.RoundToInt(moveSpeed * (percentage / 100f));
        }

        public void SpeedUp()
        {
            StartCoroutine(SpeedUpCoroutine());
        }

        private IEnumerator SpeedUpCoroutine()
        {
            moveSpeed *= 3; // Double the move speed
            yield return new WaitForSeconds(3); // Wait for 3 seconds
            moveSpeed = originalMoveSpeed; // Revert to the original move speed
        }

        /*
        private void PreventPlayerGoingOffScreen()
        {
            // Define the boundaries of the rectangle
            float minX = -48f; // Minimum X coordinate
            float maxX = 48f;  // Maximum X coordinate
            float minY = -34.5f; // Minimum Y coordinate
            float maxY = 34.5f;  // Maximum Y coordinate

            // Clamp the player's position within the defined boundaries
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

            // Update the player's position to the clamped position
            transform.position = new Vector2(clampedX, clampedY);
        }
        */
    }
}
