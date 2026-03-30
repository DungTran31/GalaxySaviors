using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace DungTran31.UI
{
    public class WobblyText2 : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Gradient rainbow;
        private Vector3[] _vertices;
        private Mesh _mesh;
        private List<int> _wordIndexes;
        private List<int> _wordLengths;


        private void Start()
        {
            textMesh = GetComponent<TMP_Text>();

            _wordIndexes = new List<int>{0};
            _wordLengths = new List<int>();

            string s = textMesh.text;
            for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
            {
                //_wordLengths.Add(index - _wordIndexes[_wordIndexes.Count - 1]);
                _wordLengths.Add(index - _wordIndexes[^1]);
                _wordIndexes.Add(index + 1);
            }
            //_wordLengths.Add(s.Length - _wordIndexes[_wordIndexes.Count - 1]);
            _wordLengths.Add(s.Length - _wordIndexes[^1]);
        }

        private void Update()
        {
            textMesh.ForceMeshUpdate();
            _mesh = textMesh.mesh;
            _vertices = _mesh.vertices;

            Color[] colors = _mesh.colors;

            for (int w = 0; w < _wordIndexes.Count; w++)
            {
                int wordIndex = _wordIndexes[w];
                Vector3 offset = Wobble(Time.time + w);

                for (int i = 0; i < _wordLengths[w]; i++)
                {
                    TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex+i];

                    int index = c.vertexIndex;

                    colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + _vertices[index].x*0.001f, 1f));
                    colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + _vertices[index + 1].x*0.001f, 1f));
                    colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + _vertices[index + 2].x*0.001f, 1f));
                    colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + _vertices[index + 3].x*0.001f, 1f));

                    _vertices[index] += offset;
                    _vertices[index + 1] += offset;
                    _vertices[index + 2] += offset;
                    _vertices[index + 3] += offset;
                }
            }
            
            _mesh.vertices = _vertices;
            _mesh.colors = colors;
            textMesh.canvasRenderer.SetMesh(_mesh);
        }
        Vector2 Wobble(float time) {
            return new Vector2(Mathf.Sin(time*3.3f), Mathf.Cos(time*2.5f));
        }
    }
}
