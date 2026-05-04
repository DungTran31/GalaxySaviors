using UnityEngine;

namespace DungTran31.UI
{
    public class ScrollingBackground : MonoBehaviour
    {
        [Header("Scroll Settings")]
        [SerializeField] private float scrollSpeed = 0.5f;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Player Reference")]
        [SerializeField] private Rigidbody2D playerRb;
        [SerializeField] private float movementThreshold = 0.01f;

        [Header("Options")]
        [Tooltip("If true, background TEXTURE scrolling will be opposite to player input.")]
        [SerializeField] private bool invertScrollDirection = true;

        [Tooltip("If true, background texture scroll will be tied to player movement.")]
        [SerializeField] private bool scrollOnlyWhenMoving = true;

        private SpriteRenderer _spriteRenderer;
        private Material _material;
        private Vector2 _offset;

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // Tạo instance material riêng
            _material = _spriteRenderer.material;

            _offset = _material.GetTextureOffset(MainTex);
        }

        private void Update()
        {
            float verticalInput = GetVerticalInput();

            // 🔥 Kiểm tra player có thực sự di chuyển không
            bool isPlayerMoving = Mathf.Abs(playerRb.linearVelocity.y) > movementThreshold;

            // Di chuyển background object (nếu bạn đang dùng kiểu này)
            if (isPlayerMoving)
            {
                Move(verticalInput);
            }

            // Scroll texture
            if (scrollOnlyWhenMoving)
            {
                if (isPlayerMoving)
                    ScrollBackground(verticalInput);
            }
            else
            {
                ScrollBackground(1f);
            }
        }

        private float GetVerticalInput()
        {
            float vertical = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                vertical += 1f;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                vertical -= 1f;

            return Mathf.Clamp(vertical, -1f, 1f);
        }

        private void ScrollBackground(float verticalInput)
        {
            float dir = invertScrollDirection ? 1f : -1f;

            _offset.x = Mathf.Repeat(
                _offset.x + (scrollSpeed * verticalInput * dir * Time.deltaTime),
                1f
            );

            _material.SetTextureOffset(MainTex, _offset);
        }

        private void Move(float verticalInput)
        {
            transform.Translate(
                0f,
                verticalInput * moveSpeed * Time.deltaTime,
                0f,
                Space.World
            );
        }
    }
}