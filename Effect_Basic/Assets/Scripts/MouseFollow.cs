using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    [Range(1.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(0.01f, 1.0f)]
    public float speed = 0.1f;


    private void Update()
    {
        //Input.mousePosition;  //InputManager에서 마우스 스크린 좌표 받아오는 방법

        //InputSystem을 사용해 마우스의 스크린좌표를 받아오기
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        // 거리를 떨어트리지 않으면 카메라 위치로 이동해 카메라에 찍히지 않기 때문에 거리를 벌린다.(최소 near 이상)
        mousePosition.z = distance;   
        
        // 스크린 좌표를 월드 좌표로 변환
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);

        // 해당 목적지로 이동
        transform.position = Vector3.Lerp(transform.position, target, speed);
    }
}
