using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordWobble : MonoBehaviour
{
    private TMP_Text textMesh;
    private Mesh mesh;
    private Vector3[] vertices;
    private List<int> wordIndexes;
    private List<int> wordLengths;
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();

        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();

        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);

        StartCoroutine(WobbleCor(textMesh));
    }
    private IEnumerator WobbleCor(TMP_Text text)
    {
        while (text.color.a != 0)
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            Color[] colors = mesh.colors;

            for (int w = 0; w < wordIndexes.Count; w++)
            {
                int wordIndex = wordIndexes[w];
                Vector3 offset = Wobble(Time.time + w);

                for (int i = 0; i < wordLengths[w]; i++)
                {
                    TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];

                    int index = c.vertexIndex;

                    vertices[index] += offset;
                    vertices[index + 1] += offset;
                    vertices[index + 2] += offset;
                    vertices[index + 3] += offset;
                }
            }
            mesh.vertices = vertices;
            mesh.colors = colors;
            textMesh.canvasRenderer.SetMesh(mesh);
            yield return null;
        }
    }
    private Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f));
    }
}