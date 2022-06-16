using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //const int PlayerShipCount = 5;      // 플레이어가 가지는 배의 수

    //Ship[] ships = null;                // 플레이어의 함선
    BattleField myField = null;         // 나의 필드
    public BattleField enemyField = null;      // 적의 필드
    
    bool isTurnActionFinish = false;    // 턴 동작이 끝났는지 여부

    List<int> randomList = null;        // 다음 랜덤 공격 위치

    public bool IsTurnActionFinish
    {
        get => isTurnActionFinish;
    }
    
    public bool IsDepeat
    {
        get => myField.IsDepeat;
    }

    public void Initialize(BattleField my, BattleField enemy)
    {
        myField = my;
        enemyField = enemy;

        // 위치 정보를 가지는 원본 리스트 만들기
        List<int> original = new List<int>(100);
        for (int i = 0; i < 100; i++)
        {
            original.Add(i);
        }

        // 원본 리스트를 섞어서 만들 랜덤 리스트 만들기
        randomList = new List<int>(100);
        while (original.Count > 0)
        {
            int index = Random.Range(0, original.Count);    // 원본 리스트에서 랜덤으로 하나 선택
            int value = original[index];    // 랜덤으로 고른 값 저장
            randomList.Add(value);          // 랜덤으로 고른 값을 랜덤 리스트에 저장
            original.RemoveAt(index);       // 원본 리스트에서 랜덤으로 고른 값 제거
        }
    }

    public void Attack(Vector2Int pos)
    {
        //Debug.Log("일반 공격");
        int posValue = pos.y * BattleField.FieldSize + pos.x;   // 랜덤 리스트에서 공격할 위치를 제거하기 위해 posValue 계산
        randomList.Remove(posValue);    // 랜덤 리스트에서 이번에 공격할 위치값 제거

        enemyField.Attacked(pos);       // 적 필드에 공격
    }

    public void ForcedAttack()
    {
        //bool find = false;
        //Vector2Int pos = new Vector2Int();
        //while (!find)
        //{
        //    pos.x = Random.Range(0, BattleField.FieldSize);
        //    pos.y = Random.Range(0, BattleField.FieldSize);

        //    find = enemyField.IsAttackable(pos);
        //}
        //enemyField.Attacked(pos);

        //Debug.Log("강제 공격");
        int posValue = randomList[0];   // 랜덤 리스트의 첫번째 값 저장
        randomList.RemoveAt(0);         // 랜덤 리스트에서 첫번째 값 삭제

        Vector2Int pos = new Vector2Int(posValue%BattleField.FieldSize, posValue/BattleField.FieldSize);    // 랜덤 리스트의 값을 이용해 위치로 변환
        enemyField.Attacked(pos);       // 해당 위치 공격
    }

    private void Start()
    {
        Initialize(GameManager.Inst.LeftField, GameManager.Inst.RightField);
    }
}
