using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Turn : Turret_Base
{
    [Range(0, 180.0f)]                      // 아래 변수의 범위를 지정(슬라이더바로 UI변경)
    public float halfAngle = 10.0f;         // 포신이 회전하는 각도
    public float rotateSpeed = 90.0f;       // 초당 회전 속도
    float rotateDirection = 1.0f;           // 회전 방향 결정용 변수
    float targetAngle = 0.0f;               // 현재 각도

    private void Start()
    {
        Initiallize();
        StartFire();
    }

    private void Update()
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
        GunBase.rotation = Quaternion.Euler(0, targetAngle, 0);
    }
}
