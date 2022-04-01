using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Trace : Turret_Base
{
    [Range(0, 90.0f)]
    public float fireAngle = 5.0f;          // 발사 가능한 각도
    public float smoothness = 3.0f;         // 움직임 정도(1/3초에 360도 정도의 속도)
    private Transform target = null;        // 추적 대상

    private void Start()
    {
        Initiallize();
    }

    private void Update()
    {
        if (target != null)     // 무언가가 트리거 안에 들어와 있다.
        {
            LookTarget();       // 쏠 대상으로 방향 돌리기
            //Debug.Log($"In Angle : {CanShoot()}");            

            if (CanShoot())      // 일정 각도 안이다
            {
                StartFire();    // 쏠 대상이 있고 일정 각도안에 들어왔을 때만 발사
            }
            else
            {
                StopFire();     // 쏠 대상이 있어도 일정 각도 밖이면 즉시 발사 중지
            }
        }
        else
        {
            StopFire();         // 쏠 대상이 없으면 발사 중지
        }
    }

    private void LookTarget()
    {
        // 1. LookAt
        // 해당 트랜스폼이 목표를 바라보는 방향으로 회전시킴
        // transform.LookAt(target);

        // 2. LookRotation
        // 특정 방향을 향하는 회전을 생성
        // Vector3 dir = target.position - transform.position;
        // transform.rotation = Quaternion.LookRotation(dir);

        // 3. Quaternion.Lerp, Quaternion.Slerp
        // 보간 : 중간값을 계산
        // 보간을 응용해서 부드럽게 움직이는 연출이 가능
        Vector3 dir = target.position - transform.position; //방향백터 계산(터렛->플레이어)
        dir.y = 0;                              // 고개가 내려가는 것을 방지
        //Debug.Log(dir);
        GunBase.rotation = Quaternion.Slerp(
                GunBase.rotation,               // 시작할때의 회전 상태
                Quaternion.LookRotation(dir),   // 끝났을 때의 회전 상태
                smoothness * Time.deltaTime);   // 시작과 끝 사이 지점(0이면 시작, 1이면 끝)
    }

    private bool CanShoot()
    {
        // gunBase 방향 백터와 터렛에서 플레이어로 가는 방향 백터사이의 각도를 구함
        float angle = Vector3.Angle(GunBase.forward, target.position - transform.position);
        //Debug.Log(angle);
        return Mathf.Abs(angle) < fireAngle;    // 사이각이 일정 이하면 true, 아니면 false
    }

    // 트리거안에 다른 컬라이더가 들어왔을 때 실행되는 함수
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // 들어온게 플레이어다.
        {
            target = other.transform;               // target에 플레이어 저장
        }
    }

    // 트리거안에서 다른 컬라이더가 나갔을 때 실행되는 함수
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // 나간게 플레이어면
        {
            target = null;                          // target에 null 설정
        }
    }
}
