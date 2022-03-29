using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 1. 앞으로 나가기
    // 2. 충돌 감지 + 대상 해치우기

    public float speed = 5.0f;      // 총알의 이동 속도
    Rigidbody rigid = null;         // 움직이는 물체라 rigidbody 추가 + 이동 처리용

    // 오브젝트가 생성완료되었을 때 실행
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();  //rigidbody 저장
    }

    // 최초의 Update가 실행되기 직전에 실행
    private void Start()
    {
        rigid.velocity = transform.forward * speed; // 물체의 이동 방향과 속도 설정
    }

    // 다른 collider랑 충돌했을 때 실행되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // 태그 : 특정한 무언가를 확인할 때
        // 인터페이스 : 특정한 카테고리에 속하는 것들을 처리할 때 

        //Debug.Log(collision.gameObject.name);

        // 죽일 수 있는 대상이면 죽인다.
        IDead target = collision.gameObject.GetComponent<IDead>();
        if(target != null)
        {
            target.OnDead();    // 죽일 수 있는 오브젝트라 죽인다.
        }

        // 총알이 뭔가와 부딪쳤을 때 일어나야 할 일들
        if (rigid.useGravity == false)  // 우리가 초기에 세팅한 그대로라면
        {
            rigid.useGravity = true;    // 중력을 켜고
            Vector3 reflect = Vector3.Reflect(transform.forward, collision.GetContact(0).normal); // 반사된 벡터 계산
            rigid.AddForce(reflect * 2.0f, ForceMode.Impulse); // 부딪쳐서 튕기는 느낌 추가            
            Vector3 randomDir = new Vector3(Random.value, Random.value, Random.value);  //랜덤 방향 지정            
            rigid.AddTorque(randomDir * 5.0f, ForceMode.Impulse); // 바닥에 떨어져서 구르는 느낌 추가
            Destroy(this.gameObject, 3.0f); // 3초 뒤에 사라지기
        }
        
    }
}
