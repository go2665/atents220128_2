using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 1. 앞으로 나가기
    // 2. 충돌 감지 + 대상 해치우기
    public float speed = 5.0f;
    Rigidbody rigid = null;

    // 오브젝트가 생성완료되었을 때 실행
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // 최초의 Update가 실행되기 직전에 실행
    private void Start()
    {
        //rigid.velocity = new Vector3(0, 0, speed);
        rigid.velocity = Vector3.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 태그 : 특정한 무언가를 확인할 때
        // 인터페이스 : 특정한 카테고리에 속하는 것들을 처리할 때 

        Debug.Log(collision.gameObject.name);

        IDead target = collision.gameObject.GetComponent<IDead>();
        if(target != null)
        {
            target.OnDead();
        }

        if (rigid.useGravity == false)
        {
            rigid.useGravity = true;
            rigid.AddForce(collision.GetContact(0).normal * 2.0f, ForceMode.Impulse);            
            rigid.AddTorque(Vector3.one * 5.0f, ForceMode.Impulse);
            Destroy(this.gameObject, 3.0f);
        }
        
    }
}
