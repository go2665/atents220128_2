using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject lineBase = null;

    const int gridLineCount = 11;   // 한 변의 그리드 줄 수
    LineRenderer[] lines = new LineRenderer[gridLineCount * gridLineCount];

    private void Awake()
    {
        for(int i=0; i<gridLineCount; i++)
        {
            for(int j=0; j<gridLineCount; j++)
            {
                GameObject line = Instantiate(lineBase, transform);
                lines[i * gridLineCount + j] = line.GetComponent<LineRenderer>();

                //lines[i * gridLineCount + j].SetPosition(0, new Vector3(,,));  // 시작점 설정
                //lines[i * gridLineCount + j].SetPosition(1, new Vector3(,,));  // 도착점 설정
            }
        }
    }
}
