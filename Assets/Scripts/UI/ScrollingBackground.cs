using UnityEngine;

namespace DungTran31.UI
{
    public class ScrollingBackground : MonoBehaviour
    {
        [Header("Scroll")]
        [SerializeField] private float scrollSpeed = 0.5f;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Bounds")]
        [SerializeField] private float minY = -3f;
        [SerializeField] private float maxY = 3f;

        private SpriteRenderer _spriteRenderer;
        private Material _material;
        private Vector2 _offset;

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // Tạo instance material (tránh ảnh hưởng object khác)
            _material = _spriteRenderer.material;

            // Lấy offset hiện tại
            _offset = _material.GetTextureOffset(MainTex);
        }

        private void Update()
        {
            float verticalInput = GetVerticalInput();

            // Không có input → không làm gì
            if (Mathf.Approximately(verticalInput, 0f))
                return;

            // Nếu còn trong giới hạn → move + scroll
            if (CanMove(verticalInput))
            {
                Move(verticalInput);
                ScrollBackground();
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

        private bool CanMove(float verticalInput)
        {
            float currentY = transform.position.y;

            // Chặn đi lên khi đã chạm max
            if (verticalInput > 0 && currentY >= maxY)
                return false;

            // Chặn đi xuống khi đã chạm min
            if (verticalInput < 0 && currentY <= minY)
                return false;

            return true;
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

        private void ScrollBackground()
        {
            _offset.x = Mathf.Repeat(
                _offset.x + scrollSpeed * Time.deltaTime,
                1f
            );

            _material.SetTextureOffset(MainTex, _offset);
        }

        // (Optional) Vẽ gizmos để dễ debug trong Scene
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 pos = transform.position;

            Gizmos.DrawLine(
                new Vector3(pos.x - 5, minY, 0),
                new Vector3(pos.x + 5, minY, 0)
            );

            Gizmos.DrawLine(
                new Vector3(pos.x - 5, maxY, 0),
                new Vector3(pos.x + 5, maxY, 0)
            );
        }
    }
}