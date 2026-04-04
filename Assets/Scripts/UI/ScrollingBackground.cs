using UnityEngine;

namespace DungTran31.UI
{
    public class ScrollingBackground : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 0.5f;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Options")]
        [Tooltip("If true, background TEXTURE scrolling will be opposite to player input. (Transform movement is NOT inverted)")]
        [SerializeField] private bool invertScrollDirection = true;

        [Tooltip("If true, background texture scroll will be tied to vertical input (up/down). If false, it scrolls constantly.")]
        [SerializeField] private bool scrollOnlyWhenMoving = true;

        private SpriteRenderer _spriteRenderer;
        private Material _material;
        private Vector2 _offset;

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // Use an instance of the material so we don't modify the shared material for all objects.
            _material = _spriteRenderer.material;

            // Initialize with current material offset if any.
            _offset = _material.GetTextureOffset(MainTex);
        }

        private void Update()
        {
            float verticalInput = GetVerticalInput();

            // Movement: keep it as-is (W moves up, S moves down).
            if (!Mathf.Approximately(verticalInput, 0f))
                Move(verticalInput);

            // Scrolling: invert only the TEXTURE scrolling direction as requested.
            if (scrollOnlyWhenMoving)
            {
                if (!Mathf.Approximately(verticalInput, 0f))
                    ScrollBackground(verticalInput);
            }
            else
            {
                // Constant scroll (use 1 so it scrolls at scrollSpeed)
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
            // Invert ONLY scroll direction (not object movement).
            // When moving down (verticalInput = -1), with invertScrollDirection = true,
            // this becomes +scroll, i.e. scrolls the opposite way.
            float dir = invertScrollDirection ? 1f : -1f;

            // Using Repeat keeps offset stable in [0..1).
            _offset.x = Mathf.Repeat(_offset.x + (scrollSpeed * verticalInput * dir * Time.deltaTime), 1f);
            _material.SetTextureOffset(MainTex, _offset);
        }

        private void Move(float verticalInput)
        {
            transform.Translate(0f, verticalInput * moveSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}