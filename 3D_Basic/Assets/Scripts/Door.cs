using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim = null;   //애니메이터
    private bool isOpen = false;
    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
    }
    protected bool isEnterFront = false;    // true : 대상이 앞쪽으로 들어오고 있다.  false : 대상이 뒤쪽으로 들어오고 있다.

    private void Awake()
    {
        anim = GetComponent<Animator>();    //애니메이터 캐싱
    }

    // 문이 열릴때 실행되는 함수
    public virtual bool Open()
    {
        //Debug.Log("Door Open");
        if (isEnterFront)   
        {
            // front가 true면 문 앞에 플레이어가 있다.
            anim.SetTrigger("Open_Front");  // 문을 앞에서 뒤쪽으로 열라는 트리거
        }
        else  
        {
            // front가 false면 문 뒤에 플레이어가 있다.
            anim.SetTrigger("Open_Back");   // 문을 뒤쪽에서 앞으로 열라는 트리거
        }
        isOpen = true;

        return isOpen;
    }

    // 문이 닫힐때 실행되는 함수
    public virtual bool Close()
    {
        //Debug.Log("Door Close");
        anim.SetTrigger("Close");   // 문을 닫으라는 트리거
        isOpen = false;

        return isOpen;
    }

    public void ToggleOpenClose()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    // 문 앞에 있는지 뒤에 있는지 확인하는 함수
    protected void CheckFront(Transform target)   // target은 문으로 들어가려는 오브젝트의 트랜스폼
    {        
        Vector3 dir = transform.position - target.position;     // 문으로 진입하는 방향벡터 구하기
        float angle = Vector3.Angle(dir, transform.forward);    // 진입 방향벡터와 문의 forward 벡터의 사이각 구하기
        if (angle > 90.0f && angle < 180.0f)    // 사이각이 둔각일 경우
        {
            isEnterFront = true;    //문 앞에 있는 것으로 판정
        }
        else
        {
            isEnterFront = false;   //문 뒤에 있는 것으로 판정
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player 태그를 가진 오브젝트만 처리
        {
            CheckFront(other.transform);
        }
    }
}
