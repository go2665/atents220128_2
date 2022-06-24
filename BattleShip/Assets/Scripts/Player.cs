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

    //bool lastActtackSuccess = false;
    Ship lastHitShip = null;
    Vector2Int lastHitPosition;
    Stack<Vector2Int> attackCandidate = new();
    readonly Vector2Int[] neighbor = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

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
        enemyField.OnAliveShipCountChange += OnEnemyShipDestroyed;

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

            lastHitShip = enemyField.Attacked(pos);       // 적 필드에 공격
            if(lastHitShip && !lastHitShip.IsSinking)
            {
                AddAttackCandidate(pos);
            }
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
            if( attackCandidate.Count > 0)
            {
                // 이전 공격이 명중했다.
                Vector2Int pos = attackCandidate.Pop();
                randomList.Remove(pos.x + pos.y * BattleField.FieldSize);

                // 테스트용------------
                Destroy(candidateMarks[pos]);
                candidateMarks.Remove(pos);
                // -------------------

                lastHitShip = enemyField.Attacked(pos);       // 해당 위치 공격
                if (lastHitShip && !lastHitShip.IsSinking)
                {
                    AddAttackCandidate(pos);
                    lastHitPosition = pos;
                }
                isTurnActionFinish = true;
            }
            else
            {
                // 이전 공격이 실패했다.
                //Debug.Log($"{gameObject.name} 강제 공격");
                int posValue = randomList[0];   // 랜덤 리스트의 첫번째 값 저장
                randomList.RemoveAt(0);         // 랜덤 리스트에서 첫번째 값 삭제

                Vector2Int pos = new(posValue % BattleField.FieldSize, posValue / BattleField.FieldSize);    // 랜덤 리스트의 값을 이용해 위치로 변환
                lastHitShip = enemyField.Attacked(pos);       // 해당 위치 공격
                if (lastHitShip && !lastHitShip.IsSinking)
                {
                    AddAttackCandidate(pos);
                    lastHitPosition = pos;
                }
                isTurnActionFinish = true;
            }            
        }
    }

    public GameObject attackCandidateMark = null;
    Dictionary<Vector2Int, GameObject> candidateMarks = new();
    void AddAttackCandidate(Vector2Int pos)
    {
        // 새 공격 후보 뽑기
        List<Vector2Int> temp = new(4);
        foreach (var n in neighbor)
        {
            Vector2Int nPos = pos + n;                  // 공격한 위치의 위아래좌우 구하기
            if (enemyField.IsValidAndAttackable(nPos))  // 공격 불가능한 부분 제거
            {
                temp.Add(nPos);
            }
        }
        do
        {
            int index = Random.Range(0, temp.Count); 
            
            if (!candidateMarks.ContainsKey(temp[index]))
            {
                attackCandidate.Push(temp[index]); // 랜덤으로 섞어 넣기                        

                // 테스트용------------
                GameObject obj = Instantiate(attackCandidateMark);
                obj.transform.position = enemyField.transform.position + new Vector3(temp[index].x + 0.5f, 0.0f, -temp[index].y - 0.5f);
                obj.transform.Translate(Vector3.up * 0.5f, Space.World);
                //--------------------
                candidateMarks[temp[index]] = obj;

            }
            temp.RemoveAt(index);
            
        } while (temp.Count > 0);

        lastHitPosition = pos;
    }
    private void OnEnemyShipDestroyed(int _)
    {
        attackCandidate.Clear();

        // 테스트용---------------------------
        foreach(var mark in candidateMarks)
        {
            Destroy(mark.Value);
        }
        candidateMarks.Clear();
        // -----------------------------------
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
