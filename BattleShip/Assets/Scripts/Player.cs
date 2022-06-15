using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //const int PlayerShipCount = 5;      // 플레이어가 가지는 배의 수

    //Ship[] ships = null;                // 플레이어의 함선
    BattleField myField = null;         // 나의 필드
    BattleField enemyField = null;      // 적의 필드
    
    bool isTurnActionFinish = false;    // 턴 동작이 끝났는지 여부
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
    }

    public void Attack(Vector2Int pos)
    {
        enemyField.Attacked(pos);
    }

    public void ForcedAttack()
    {
        bool find = false;
        Vector2Int pos = new Vector2Int();
        while (!find)
        {
            pos.x = Random.Range(0, BattleField.FieldSize);
            pos.y = Random.Range(0, BattleField.FieldSize);

            find = enemyField.IsAttackable(pos);
        }
        enemyField.Attacked(pos);
    }

    private void Start()
    {
        Initialize(GameManager.Inst.LeftField, GameManager.Inst.RightField);
    }
}
