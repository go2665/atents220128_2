using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Auto : Door
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player 태그를 가진 오브젝트만 처리
        {
            if (!IsOpen)
            {
                CheckFront(other.transform);
                Open();    // 문 열기
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Player 태그를 가진 오브젝트만 처리
        {
            if (IsOpen)
            {
                Close();
            }
        }
    }
}
