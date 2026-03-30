using UnityEngine;

namespace DungTran31.UI
{
    public class SetSortingLayer : MonoBehaviour
    {
        [SerializeField] private string sortingLayerName = "Foreground";
        [SerializeField] private int orderInLayer = 1;

        private void Start()
        {
            if (TryGetComponent<TextMesh>(out _))
            {
                if (TryGetComponent<Renderer>(out Renderer textRenderer))
                {
                    textRenderer.sortingLayerName = sortingLayerName;
                    textRenderer.sortingOrder = orderInLayer;
                }
            }
        }
    }
}
