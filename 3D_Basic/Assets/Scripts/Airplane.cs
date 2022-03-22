using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//실습(2시까지)
// 비행기 게임 오브젝트 만들기(프리팹)
// 이름 : Airplane
// 방식 : 프리미티브 사용 or 프로빌더
// 스크립트 완성(propeller가 true면 프로펠러가 돌아가고 false 안돌아간다.)

public class Airplane : MonoBehaviour
{
    public bool propellerOn = false;    // true면 프로펠러가 돌아가고 false 안돌아간다.
    public float propSpeed = 720.0f;    // 1초에 2바퀴 돌리기가 기본
    
    private Transform propTransform = null; // 프로펠러의 트랜스폼

    // 오브젝트가 생성이 완료됬을 때 실행되는 함수
    private void Awake()
    {
        //transform에 자식들 중에서 이름이 "Propeller"인 트랜스폼 찾기
        propTransform = transform.Find("Propeller");
    }

    private void Update()
    {
        if (propellerOn)    // propellerOn이 true일때만 돌리기
        {
            propTransform.Rotate(0, 0, propSpeed * Time.deltaTime); // 프로펠러의 트랜스폼을 돌리기
        }
    }

    // Inspector창에서 값이 성공적으로 변경되었을 때 실행
    //private void OnValidate()
    //{        
    //}
}
