using UnityEngine;

namespace DungTran31.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class ChainRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject ball;
        [SerializeField] private int numberOfSegments = 10;
        [SerializeField] private float chainWidth = 0.5f;
        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = numberOfSegments;
            lineRenderer.startWidth = chainWidth;
            lineRenderer.endWidth = chainWidth;
        }

        private void Update()
        {
            if (player == null || ball == null)
            {
                lineRenderer.enabled = false;
                return;
            }

            lineRenderer.enabled = true;
            UpdateChain();
        }

        private void UpdateChain()
        {
            Vector3 startPosition = player.transform.position;
            Vector3 endPosition = ball.transform.position;

            for (int i = 0; i < numberOfSegments; i++)
            {
                float t = (float)i / (numberOfSegments - 1);
                Vector3 chainPosition = Vector3.Lerp(startPosition, endPosition, t);
                lineRenderer.SetPosition(i, chainPosition);
            }
        }
    }
}
