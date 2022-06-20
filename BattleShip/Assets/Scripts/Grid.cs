using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid : MonoBehaviour
{
    public GameObject lineBase = null;
    public GameObject letter = null;

    const int gridCount = 10;               // 한 변의 그리드 칸 수
    const int gridLineCount = gridCount+1;  // 한 변의 그리드 줄 수
    //const int gridLineTotalCount = gridLineCount * 2;
    //LineRenderer[] lines = new LineRenderer[gridLineTotalCount];

    private void Awake()
    {
        // 세로선 긋기
        for (int i=0; i< gridLineCount; i++)
        {
            GameObject line = Instantiate(lineBase, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            
            lineRenderer.SetPosition(0, new Vector3(i, 0.1f, 1));  // 시작점 설정
            lineRenderer.SetPosition(1, new Vector3(i, 0.1f, -gridLineCount+1));  // 도착점 설정            
        }

        // 가로선 긋기
        for (int i=0; i < gridLineCount; i++)
        {
            GameObject line = Instantiate(lineBase, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            lineRenderer.SetPosition(0, new Vector3(-1, 0.1f,-i));  // 시작점 설정
            lineRenderer.SetPosition(1, new Vector3(gridLineCount-1, 0.1f, -i));  // 도착점 설정
        }

        for (int i = 0; i < gridCount; i++)
        {
            GameObject text = Instantiate(letter, transform);
            RectTransform rect = text.GetComponent<RectTransform>();
            rect.pivot = Vector2.zero;
            rect.position = new Vector3(i, 0.1f, 0);
            TextMeshPro textMesh = text.GetComponent<TextMeshPro>();
            char c = (char)(65 + i);
            textMesh.text = c.ToString();
        }

        for (int i = 0; i < gridCount; i++)
        {
            GameObject text = Instantiate(letter, transform);
            RectTransform rect = text.GetComponent<RectTransform>();            
            rect.pivot = Vector2.one;
            rect.position = new Vector3(0, 0.1f, -i);
            TextMeshPro textMesh = text.GetComponent<TextMeshPro>();
            textMesh.text = (i + 1).ToString();
            if (i == 9)
            {
                textMesh.fontSize = 7;
            }
        }
    }
}
