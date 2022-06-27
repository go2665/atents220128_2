using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    /// <summary>
    /// 마지막으로 공격한 배(명중 여부 표시 및 침몰 확인용.)
    /// </summary>
    Ship lastHitShip = null;

    /// <summary>
    /// 마지막으로 성공한 공격 위치(좀 더 적절한 위치를 계산하기 위해 사용. 배가 침몰할 경우 INVALID_GRID_POSITION로 초기화)
    /// </summary>
    Vector2Int lastHitPosition;

    /// <summary>
    /// 공격할 위치 후보. 배가 침몰할 경우 Clear
    /// </summary>
    Stack<Vector2Int> attackCandidate = new();    


    // 테스트용 변수 -------------------------------------------------------------------------------
    /// <summary>
    /// 공격할 위치 후보를 표시할 오브젝트(녹색판)
    /// </summary>
    public GameObject attackCandidateMark = null;

    /// <summary>
    /// attackCandidateMark들을 저장해 놓은 딕셔너리
    /// </summary>
    readonly Dictionary<Vector2Int, GameObject> candidateMarks = new();


    // 읽기 전용 -----------------------------------------------------------------------------------
    /// <summary>
    /// 잘못된 값이라는 것을 확인해주기 위한 변수
    /// </summary>
    readonly Vector2Int INVALID_GRID_POSITION = -Vector2Int.one;

    /// <summary>
    /// 공격지점의 4방향을 계산하기 위한 변수
    /// </summary>
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

        lastHitPosition = INVALID_GRID_POSITION;
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
            if (lastHitShip)    // 배를 맞췄다.
            {
                if (!lastHitShip.IsSinking)     
                {
                    // 배가 침몰은 안됬다.
                    AddAttackCandidate(pos);    // 새 후보위치 추가
                    lastHitPosition = pos;
                }
                else
                {
                    // 배가 침몰했다.
                    lastHitPosition = INVALID_GRID_POSITION;
                }
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
                if (lastHitShip)    // 배를 맞췄다.
                {
                    if (!lastHitShip.IsSinking)
                    {
                        // 배가 침몰은 안됬다.
                        AddAttackCandidate(pos);    // 새 후보위치 추가
                        lastHitPosition = pos;
                    }
                    else
                    {
                        // 배가 침몰했다.
                        lastHitPosition = INVALID_GRID_POSITION;
                    }
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
                if (lastHitShip)    // 배를 맞췄다.
                {
                    if (!lastHitShip.IsSinking)
                    {
                        // 배가 침몰은 안됬다.
                        AddAttackCandidate(pos);    // 새 후보위치 추가
                        lastHitPosition = pos;
                    }
                    else
                    {
                        // 배가 침몰했다.
                        lastHitPosition = INVALID_GRID_POSITION;
                    }
                }
                isTurnActionFinish = true;
            }            
        }
    }

    void AddAttackCandidate(Vector2Int pos)
    {
        // 새 공격 후보 뽑기
        List<Vector2Int> temp = new(4);

        if (lastHitPosition.x == pos.x)
        {
            // 한줄로 맞았다.
            Vector2Int nPos = pos;
            for (int i = pos.y - 1; i > -1; i--)
            {
                nPos.y = i;
                if(enemyField.IsAttckFailPosition(nPos))
                {
                    break;
                }
                if (enemyField.IsValidAndAttackable(nPos))
                {
                    temp.Add(nPos);
                    break;
                }
            }
            for (int i = pos.y + 1; i < BattleField.FieldSize; i++)
            {
                nPos.y = i;
                if (enemyField.IsAttckFailPosition(nPos))
                {
                    break;
                }
                if (enemyField.IsValidAndAttackable(nPos))
                {
                    temp.Add(nPos);
                    break;
                }
            }
        }
        else if( lastHitPosition.y == pos.y)
        {
            // 한줄로 맞았다.
            Vector2Int nPos = pos;
            for (int i = pos.x - 1; i > -1; i--)
            {
                nPos.x = i;
                if (enemyField.IsAttckFailPosition(nPos))
                {
                    break;
                }
                if (enemyField.IsValidAndAttackable(nPos))
                {
                    temp.Add(nPos);
                    break;
                }
            }
            for (int i = pos.x + 1; i < BattleField.FieldSize; i++)
            {
                nPos.x = i;
                if (enemyField.IsAttckFailPosition(nPos))
                {
                    break;
                }
                if (enemyField.IsValidAndAttackable(nPos))
                {
                    temp.Add(nPos);
                    break;
                }
            }
        }
        else
        {
            // 완전 별도의 공간
            foreach (var n in neighbor)
            {
                Vector2Int nPos = pos + n;                  // 공격한 위치의 위아래좌우 구하기
                if (enemyField.IsValidAndAttackable(nPos))  // 공격 불가능한 부분 제거
                {
                    temp.Add(nPos);
                }
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
