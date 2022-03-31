using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터렛 움직임 모드
public enum TurretMode
{
    STAY = 0,       // 모드1 : 가만히 있는다. 무조건 총알 발사
    TURN,           // 모드2 : 좌우로 회전하는 모드. 무조건 총알 발사
    TRACE           // 모드3 : 대상을 추적하는 모드. 범위안에 들어오면 총알 발사
}

public class Turret : MonoBehaviour
{
    // 공통    
    public TurretMode mode = TurretMode.STAY;       // 터렛의 기본 모드는 STAY
    public int additionalGunCount = 0;              // 추가될 총의 개수
    private int oldAdditionalGunCount = 0;          // additionalGunCount의 이전 값
    private Gun[] guns = null;                      // 터렛의 총들
    private Transform gunBase = null;               // 총들이 부모 transform
    private GameObject gunObject = null;            // 기본 총 오브젝트(복사될 원본)
    private Queue<GameObject> additionalGuns = new Queue<GameObject>(0);    // 추가로 생성된 총들의 목록

    // TurnMode용 변수
    [Header("TurnMode용 변수")]              // 인스펙터 창에 해더 추가
    [Range(0, 180.0f)]                      // 아래 변수의 범위를 지정(슬라이더바로 UI변경)
    public float halfAngle = 10.0f;         // 포신이 회전하는 각도
    public float rotateSpeed = 90.0f;       // 초당 회전 속도
    float rotateDirection = 1.0f;           // 회전 방향 결정용 변수
    float targetAngle = 0.0f;               // 현재 각도

    // TraceMode용 변수
    [Header("TraceMode용 변수")]
    [Range(0, 90.0f)]
    public float fireAngle = 5.0f;          // 발사 가능한 각도
    public float smoothness = 3.0f;         // 움직임 정도(1/3초에 360도 정도의 속도)
    private Transform target = null;        // 추적 대상
    
    // 오브젝트가 생성완료되었을 때 실행되는 함수
    private void Awake()
    {
        gunBase = transform.Find("GunBase");            // 총이 자식으로 붙을 gunBase 찾기
        gunObject = gunBase.GetChild(0).gameObject;     // 복제될 원본 총 오브젝트 찾기
        AddGuns();                                      // additionalGunCount에 따라 총 생성        
    }    

    // 업데이트 직전에 호출되는 함수
    private void Start()
    {
        InitializeGun(1.0f, 3, 0.1f);   // 총의 특성 설정(발사 인터벌과 연사 등)
        switch (mode)
        {
            case TurretMode.STAY:
            case TurretMode.TURN:
                StartFire();            //Stay와 Turn모드는 시작하면 무조건 발사
                break;
            case TurretMode.TRACE:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (mode)               // 각각의 모드에 맞는 업데이트 함수 실행
        {
            case TurretMode.STAY:
                Update_StayMode();
                break;
            case TurretMode.TURN:
                Update_TurnMode();
                break;
            case TurretMode.TRACE:
                Update_TraceMode();
                break;
            default:
                break;
        }
    }

    // 총의 발사 인터벌, 연사수, 연사간격 설정
    void InitializeGun(float interval, int shots, float rateOfFire)
    {
        foreach (Gun gun in guns)   // guns에 들어있는 모든 총들을 초기화
        {
            gun.Initialize(interval, shots, rateOfFire);    // 총 특성 초기화
        }
    }

    void Update_StayMode()
    {
        // 하는일 없음
    }    

    void Update_TurnMode()
    {
        // targetAngle은 매프레임 증가(증가 방향은 rotateDirection에 의해 결정된다)
        targetAngle += rotateDirection * rotateSpeed * Time.deltaTime;
        targetAngle = Mathf.Clamp(targetAngle, -halfAngle, halfAngle);

        // targetAngle의 절대값이 halfAngle을 넘어섰는지 확인
        if (Mathf.Abs(targetAngle) >= halfAngle)
        {
            rotateDirection *= -1.0f;   // 방향 뒤집기
        }
        //Debug.Log(targetAngle);
        // 실제 회전은 여기서 적용. y축으로 targetAngle만큼 회전하는 쿼터니언 생성
        // 생성한 쿼터니언을 기둥의 회전으로 적용
        gunBase.rotation = Quaternion.Euler(0, targetAngle, 0);
    }

    void Update_TraceMode()
    {
        if (target != null)     // 무언가가 트리거 안에 들어와 있다.
        {
            LookTarget();       // 쏠 대상으로 방향 돌리기
            //Debug.Log($"In Angle : {CanShoot()}");            

            if(CanShoot())      // 일정 각도 안이다
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
        gunBase.rotation = Quaternion.Slerp(
                gunBase.rotation,               // 시작할때의 회전 상태
                Quaternion.LookRotation(dir),   // 끝났을 때의 회전 상태
                smoothness * Time.deltaTime);   // 시작과 끝 사이 지점(0이면 시작, 1이면 끝)
    }

    private bool CanShoot()
    {
        // gunBase 방향 백터와 터렛에서 플레이어로 가는 방향 백터사이의 각도를 구함
        float angle = Vector3.Angle(gunBase.forward, target.position - transform.position);
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

    // 터렛에 달린 총 발사
    public void StartFire()
    {
        foreach (Gun gun in guns)   // guns에 있는 모든 총을 발사
        {
            gun.StartFire();
        }
    }

    // 터렛에 달린 총 발사 중지
    public void StopFire()
    {
        foreach (Gun gun in guns)   // guns에 있는 모든 총 발사 중지
        {
            gun.StopFire();   
        }
    }

    // additionalGunCount만큼 총을 추가해주는 함수
    private void AddGuns()
    {
        additionalGuns = new Queue<GameObject>(additionalGunCount); // 추가한 총을 저장할 콜렉션 생성

        for (int i = 0; i < additionalGunCount; i++)                // additionalGunCount만큼 반복
        {
            GameObject cloneGun = Instantiate(gunObject, gunBase);  // 총 오브젝트 클로닝하고 gunBase의 자식으로 추가
            cloneGun.transform.Translate(cloneGun.transform.up * (i + 1) * 0.2f); // 새로 추가한 총을 위로 붙임
            additionalGuns.Enqueue(cloneGun);                       // 큐 콜렉션에 총 추가
        }

        guns = gunBase.GetComponentsInChildren<Gun>();              // gunBase의 자식으로 있는 모든 총 찾기
    }

    // additionalGuns에 있는 모든 총을 삭제
    private void RemoveGuns()
    {
        while (additionalGuns.Count > 0)    // additionalGuns에 들어있는 것이 있으면 계속 실행
        {
            GameObject clonGun = additionalGuns.Dequeue();  // additionalGuns에 들어있는 것을 하나 꺼내고
            Destroy(clonGun);               // 삭제
        }
    }

    //// 인스펙터 창에서 값이 정상적으로 변경되었을 때 실행
    //private void OnValidate()
    //{
    //    if (additionalGunCount != oldAdditionalGunCount)    // additionalGunCount가 변경되었을 때만 실행
    //    {
    //        // gunBase나 gunObject가 null이면 찾아서 저장
    //        if (gunBase == null)
    //        {
    //            gunBase = transform.Find("GunBase");
    //        }
    //        if (gunObject == null)
    //        {
    //            gunObject = gunBase.GetChild(0).gameObject;
    //        }
    //        RemoveGuns();   // 기존에 추가된 총 모두 삭제
    //        AddGuns();      // 새롭게 총 추가
    //        oldAdditionalGunCount = additionalGunCount;     // oldAdditionalGunCount 갱신
    //    }
    //}
}
