using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터렛 움직임 모드
public enum TurretMode
{
    STAY = 0,       // 모드1 : 가만히 있는다.
    TURN,           // 모드2 : 좌우로 회전하는 모드.
    TRACE           // 모드3 : 대상을 추적하는 모드.
}

public class Turret : MonoBehaviour
{
    // 공통    
    public TurretMode mode = TurretMode.STAY;      // 터렛의 기본 모드는 STAY
    private Gun[] guns = null;                        // 터렛의 총

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

    // 총알 발사 조건
    // 1. 모조건 발사
    // 2. 특정 상황에서만 발사

    private void Awake()
    {
        guns = transform.GetComponentsInChildren<Gun>();  // 자식으로 있는 총 찾기
    }

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
                StayMode();
                break;
            case TurretMode.TURN:
                TurnMode();
                break;
            case TurretMode.TRACE:
                TraceMode();
                break;
            default:
                break;
        }
    }

    void InitializeGun(float interval, int shots, float rateOfFire)
    {
        foreach (Gun gun in guns)
        {
            gun.Initialize(interval, shots, rateOfFire);    // 총 특성 초기화
        }
    }

    void StayMode()
    {
        // 하는일 없음
    }    

    void TurnMode()
    {
        if (guns != null)
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
            foreach (Gun gun in guns)
            {
                gun.transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }
    }

    void TraceMode()
    {
        if (target != null)  // 무언가가 트리거 안에 들어와 있다.
        {
            LookTarget();   // 방향 돌리기
            //Debug.Log($"In Angle : {CanShoot()}");            

            if(CanShoot())
            {
                StartFire();    // 쏠 대상이 있고 일정 각도안에 들어왔을 때만 발사
            }
            else
            {
                StopFire();
            }
        }
        else
        {
            StopFire();
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
        dir.y = 0;
        Debug.Log(dir);
        foreach (Gun gun in guns)
        {
            gun.transform.rotation =
                Quaternion.Slerp(
                    gun.transform.rotation,             // 시작할때의 회전 상태
                    Quaternion.LookRotation(dir),   // 끝났을 때의 회전 상태
                    smoothness * Time.deltaTime);   // 시작과 끝 사이 지점(0이면 시작, 1이면 끝)
        }
    }

    private bool CanShoot()
    {
        // 총구 방향 백터와 터렛에서 플레이어로 가는 방향 백터사이의 각도를 구함
        float angle = Vector3.Angle(guns[0].transform.forward, target.position - transform.position);
        //Debug.Log(angle);
        return Mathf.Abs(angle) < fireAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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

    // 터렛에 달린 총 발사
    public void StartFire()
    {
        foreach (Gun gun in guns)
        {
            gun.StartFire();
        }
    }

    // 터렛에 달린 총 발사 중지
    public void StopFire()
    {
        foreach (Gun gun in guns)
        {
            gun.StopFire();
        }
    }
}
