using UnityEngine;
using TMPro;
namespace DungTran31.UI
{
    public class WobblyText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent;

        private void Update()
        {
            textComponent.ForceMeshUpdate();
            var textInfo = textComponent.textInfo;

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                var charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                {
                    continue;
                }

                var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

                for (int j = 0; j < 4; ++j)
                {
                    var index = charInfo.vertexIndex + j;
                    var orig = meshInfo.vertices[index];
                    meshInfo.vertices[index] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
                    meshInfo.colors32[index] = Color.red;
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; ++i)
            {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                textComponent.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }
}
