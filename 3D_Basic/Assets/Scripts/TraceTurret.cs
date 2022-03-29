using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceTurret : MonoBehaviour
{
    public GameObject bullet = null;        // 발사할 총알 오브젝트
    public Transform shotTransform = null;  // 총알이 발사될 위치

    public float interval = 1.0f;           // 총알을 발사하는 간격(방아쇠를 당기는 간격)
    public int shots = 5;                   // 한번 발사할때 몇연사를 할 것인가
    public float rateOfFire = 0.1f;         // 연사를 할 때 발사되는 간격

    public float fireAngle = 5.0f;
    public float smoothness = 3.0f;
     
    IEnumerator shotSave;                   // 코루틴용 IEnumerator 저장
    
    Transform target = null;

    //private void Awake()
    //{
    //    SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
    //}

    // 첫번째 업데이트가 실행되기 직전
    private void Start()
    {
        shotSave = Shot();          // IEnumerator 저장
        StartCoroutine(shotSave);   // 저장한 IEnumerator로 코루틴 실행
    }

    private void Update()
    {
        if (target != null)  // 무언가가 트리거 안에 들어와 있다.
        {
            LookTarget();   // 방향 돌리기
            //Debug.Log($"In Angle : {CanShoot()}");            
        }
    }

    private bool CanShoot()
    {
        // 총구 방향 백터와 터렛에서 플레이어로 가는 방향 백터사이의 각도를 구함
        float angle = Vector3.Angle(shotTransform.forward, target.position - transform.position);
        //Debug.Log(angle);
        return Mathf.Abs(angle) < fireAngle;
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
        dir.y = 0;
        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,             // 시작할때의 회전 상태
                Quaternion.LookRotation(dir),   // 끝났을 때의 회전 상태
                smoothness * Time.deltaTime);   // 시작과 끝 사이 지점(0이면 시작, 1이면 끝)
    }

    IEnumerator Shot()
    {
        while (true)
        {
            if (target != null && CanShoot())   // 대상이 있고 쏠 수 있을때만 진행
            {
                yield return new WaitForSeconds(interval - shots * rateOfFire); // 1초-0.1초*5 대기
                                                                                // 총알 연사 시작
                for (int i = 0; i < shots; i++)
                {
                    //GameObject bulletInstance = Instantiate(bullet, shotTransform);  // 총알 생성
                    //bulletInstance.transform.parent = null;
                    Instantiate(bullet, shotTransform.position, shotTransform.rotation);

                    yield return new WaitForSeconds(rateOfFire);    // 0.1초 대기
                }
            }
            yield return null;  // 다음 프레임으로(Update 함수처럼 사용중)
        }
    }

    // 일정 범위 안에 들어왔는지를 확인하는 방법
    //  1. 플레이어와의 거리를 계산한다.
    //     단점 : 매 프레임 계산해야 한다.
    //  2. 트리거를 사용한다.

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = null;            
        }
    }
}
