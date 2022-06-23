using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //const int PlayerShipCount = 5;      // 플레이어가 가지는 배의 수
    //Ship[] ships = null;                // 플레이어의 함선

    // 주요 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// 나의 필드
    /// </summary>
    BattleField myField = null;
    /// <summary>
    /// 적의 필드
    /// </summary>
    BattleField enemyField = null;

    /// <summary>
    /// 턴 동작 완료 여부
    /// </summary>
    bool isTurnActionFinish = false;

    /// <summary>
    /// 랜덤 공격용 데이터(미리 계산해 놓음)
    /// </summary>
    List<int> randomList = null;

    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 턴 동작 완료 여부 프로퍼티
    /// </summary>
    public bool IsTurnActionFinish
    {
        get => isTurnActionFinish;
    }

    /// <summary>
    /// 패배 여부 확인(내 필드에 남은 배가 있는지 확인)
    /// </summary>
    public bool IsDepeat
    {
        get => myField.IsDepeat;
    }

    // 함수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 초기화용 함수. 필드 설정. 랜덤 리스트 설정
    /// </summary>
    /// <param name="my">내 필드</param>
    /// <param name="enemy">적 필드</param>
    public void Initialize(BattleField my, BattleField enemy)
    {
        myField = my;
        enemyField = enemy;

        // 위치 정보를 가지는 원본 리스트 만들기
        List<int> original = new(100);
        for (int i = 0; i < 100; i++)
        {
            original.Add(i);
        }

        // 원본 리스트를 섞어서 만들 랜덤 리스트 만들기
        randomList = new(100);
        while (original.Count > 0)
        {
            int index = Random.Range(0, original.Count);    // 원본 리스트에서 랜덤으로 하나 선택
            int value = original[index];    // 랜덤으로 고른 값 저장
            randomList.Add(value);          // 랜덤으로 고른 값을 랜덤 리스트에 저장
            original.RemoveAt(index);       // 원본 리스트에서 랜덤으로 고른 값 제거
        }
    }

    /// <summary>
    /// 플레이어의 공격 함수
    /// </summary>
    /// <param name="pos">공격할 위치</param>
    public void Attack(Vector2Int pos)
    {
        if (!isTurnActionFinish)  // 테스트를 위해 임시로 막음
        {
            //Debug.Log("일반 공격");
            int posValue = pos.y * BattleField.FieldSize + pos.x;   // 랜덤 리스트에서 공격할 위치를 제거하기 위해 posValue 계산
            randomList.Remove(posValue);    // 랜덤 리스트에서 이번에 공격할 위치값 제거

            enemyField.Attacked(pos);       // 적 필드에 공격

            // 테스트를 위해 임시로 막음
            isTurnActionFinish = true;      // 한턴에 한번만 공격하도록 설정
        }
    }

    /// <summary>
    /// 플레이어의 강제 공격. Timeout이나 AI 용도로 사용
    /// </summary>
    public void ForcedAttack()
    {
        if (!isTurnActionFinish)
        {
            Debug.Log($"{gameObject.name} 강제 공격");
            int posValue = randomList[0];   // 랜덤 리스트의 첫번째 값 저장
            randomList.RemoveAt(0);         // 랜덤 리스트에서 첫번째 값 삭제

            Vector2Int pos = new(posValue % BattleField.FieldSize, posValue / BattleField.FieldSize);    // 랜덤 리스트의 값을 이용해 위치로 변환
            enemyField.Attacked(pos);       // 해당 위치 공격
            isTurnActionFinish = true;
        }
    }

    /// <summary>
    /// 턴이 시작될 때 플레이어에서 리셋할 데이터들 리셋
    /// </summary>
    public void TurnStartReset()
    {
        isTurnActionFinish = false;
    }

    public bool IsMyField(BattleField field)
    {
        return (myField == field);
    }

    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    
}
