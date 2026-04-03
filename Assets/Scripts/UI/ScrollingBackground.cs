using UnityEngine;
using DungTran31.GamePlay.Player;

namespace DungTran31.UI
{
    public class ScrollingBackground : MonoBehaviour
    {
        [Header("Scroll")]
        [SerializeField] private float scrollSpeed = 0.5f;

        [Header("Renderer (optional)")]
        [SerializeField] private Renderer targetRenderer;

        [Header("Follow Player")]
        [SerializeField] private Transform player;
        [SerializeField] private bool followX = false;
        [SerializeField] private bool followY = true;
        [SerializeField] private bool followZ = false;
        [SerializeField] private Vector3 followOffset = Vector3.zero;
        [SerializeField, Tooltip("How quickly the background moves to the player's position. Higher = snappier.")]
        private float followSmooth = 15f;

        private Material _material;
        private Vector2 _offset;

        private void Awake()
        {
            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>();

            if (targetRenderer != null)
                _material = targetRenderer.material; // creates an instance for this renderer

            // Auto-find player if not assigned (requires PlayerController in scene)
            if (player == null)
            {
                var playerController = FindObjectOfType<PlayerHub>();
                if (playerController != null)
                    player = playerController.transform;
            }
        }

        private void Update()
        {
            TickScroll();
            TickFollow();
        }

        private void TickScroll()
        {
            if (_material == null)
                return;

            _offset.x = (_offset.x + scrollSpeed * Time.deltaTime) % 1f;
            _material.mainTextureOffset = _offset;
        }

        private void TickFollow()
        {
            if (player == null)
                return;

            var current = transform.position;
            var target = current;

            var desired = player.position + followOffset;

            if (followX) target.x = desired.x;
            if (followY) target.y = desired.y;
            if (followZ) target.z = desired.z;

            if (followSmooth <= 0f)
            {
                transform.position = target;
                return;
            }

            transform.position = Vector3.Lerp(current, target, 1f - Mathf.Exp(-followSmooth * Time.deltaTime));
        }

        private void OnDestroy()
        {
            // Prevent leaking the instanced material created by .material
            if (_material != null)
                Destroy(_material);
        }
    }
}