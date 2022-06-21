using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 처음 그리드 그리기 위한 코드
/// 지금은 사용 안함.
/// </summary>
public class Grid : MonoBehaviour
{
    /// <summary>
    /// 선 프리팹
    /// </summary>
    public GameObject lineBase = null;

    /// <summary>
    /// 글자(1개용) 프리팹
    /// </summary>
    public GameObject letter = null;

    /// <summary>
    /// 한 변의 그리드 칸 수(10x10)
    /// </summary>
    const int gridCount = 10;

    /// <summary>
    /// 한 변의 그리드 줄 수(칸 수보다 하나 더 많아야 한다.)
    /// </summary>
    const int gridLineCount = gridCount+1;

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

        // A~J까지 쓰기
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

        // 1~10까지 쓰기
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
