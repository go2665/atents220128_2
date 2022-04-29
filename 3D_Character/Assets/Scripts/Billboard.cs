using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // 빌보드의 특징
    // 항상 정면이 보인다 => 항상 카메라를 마주보고 있다. => 카메라의 방향벡터와 빌보드의 방향벡터가 일치해야 한다.

    Transform cameraTransform = null;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        //transform.LookAt(transform.position + cameraTransform.forward);
        //transform.forward = cameraTransform.forward;
        transform.rotation = cameraTransform.rotation;
    }
}
