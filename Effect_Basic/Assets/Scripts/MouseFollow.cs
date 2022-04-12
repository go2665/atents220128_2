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
        //Debug.Log(Mouse.current.position.ReadValue());
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = distance;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = Vector3.Lerp(transform.position, target, speed);
    }
}
