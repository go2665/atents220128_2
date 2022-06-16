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

        List<int> original = new List<int>(100);
        for (int i = 0; i < 100; i++)
        {
            original.Add(i);
        }

        randomList = new List<int>(100);
        while (original.Count > 0)
        {
            int index = Random.Range(0, original.Count);
            int value = original[index];
            randomList.Add(value);
            original.RemoveAt(index);
        }
    }

    public void Attack(Vector2Int pos)
    {
        //Debug.Log("일반 공격");
        int posValue = pos.y * BattleField.FieldSize + pos.x;
        randomList.Remove(posValue);

        enemyField.Attacked(pos);
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
        int posValue = randomList[0];
        randomList.RemoveAt(0);

        Vector2Int pos = new Vector2Int(posValue%BattleField.FieldSize, posValue/BattleField.FieldSize);
        enemyField.Attacked(pos);

    }

    private void Start()
    {
        Initialize(GameManager.Inst.LeftField, GameManager.Inst.RightField);
    }
}
