using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어(인간, 컴퓨터, 은행 포함)
/// </summary>
public class Player : MonoBehaviour
{
    DiceSet dice;
    Map map;

    const int StartMoney = 5000;        // 시작 금액

    PlayerType type = PlayerType.Bank;  // 플레이어 타입 설정

    int money;                          // 보유 금액
    int actionCount = 1;                // 행동력
    int islandWaitTime = -1;            // 무인도 대기 시간
    MapID position = MapID.Start;       // Start 위치로 초기 위치 설정
    
    Material material;                  // 플레이어의 색상을 지정하기 위해 사용

    /// <summary>
    /// 보유 금액 프로퍼티
    /// </summary>
    public int Money 
    { 
        get => money; 
        set
        {
            if (money != value)
            {
                OnMoneyChange?.Invoke(money);   // 금액에 변동이 있으면 OnMoneyChange 델리게이트 실행
            }
            money = value;
        }
    }

    /// <summary>
    /// 플레이어의 위치용 프로퍼티
    /// </summary>
    public MapID Position
    {
        get => position;
        set
        {
            position = value;
        }
    }

    /// <summary>
    /// 플레이어의 종류 프로퍼티(읽기전용)
    /// </summary>
    public PlayerType Type
    {
        get => type;
    }

    /// <summary>
    /// 행동력 다 사용했는지 표시하는 프로퍼티(읽기전용)
    /// </summary>
    public bool ActionDone
    {
        get => actionCount < 1;
    }

    /// <summary>
    /// 돈에 변화가 있을 때 실행될 델리게이트
    /// </summary>
    public System.Action<int> OnMoneyChange;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    /// <summary>
    /// 초기화 함수(반드시 주사위와 맵이 초기화 된 이후에 실행되어야 함)
    /// </summary>
    /// <param name="playerType">이 플레이어가 초기화 될 타입</param>
    public void Initialize(PlayerType playerType)
    {
        type = playerType;
        material.color = GameManager.Inst.PlayerColor[(int)type];  // 플레이어의 색상 설정
        if (type == PlayerType.Bank)
        {
            // 은행일 경우
            Money = 2000000000;
            gameObject.SetActive(false);
        }
        else
        {
            Money = StartMoney;  
        }

        gameObject.name = $"Player_{playerType}";   // 플레이어 이름 설정

        map = GameManager.Inst.GameMap;
        dice = GameManager.Inst.GameDiceSet;        
        GameManager.Inst.GameDiceSet.OnDouble += OnDouble;  // 더블이 발생했을 때 실행될 함수 등록
    }

    /// <summary>
    /// 무인도 도착했을 때 실행될 함수
    /// </summary>
    /// <param name="wait">기다리는 시간(기본3)</param>
    public void OnArriveIsland(int wait)
    {
        islandWaitTime = wait;
    }

    /// <summary>
    /// 턴이 종료될 때 실행될 함수. 턴 매니저에서 호출
    /// </summary>
    public void OnTurnEnd()
    {
        //if (type == PlayerType.Human)
        //{
        //    Debug.Log($"{type} turn end");
        //}
        actionCount = 1;    // 행동력 1로 회복
        islandWaitTime--;   // 무인도 대기 시간 감소
    }

    /// <summary>
    /// 더블이 나왔을 때 실행될 함수
    /// </summary>
    /// <param name="diceThrower"></param>
    void OnDouble(PlayerType diceThrower)
    {
        if (diceThrower == type)    // 더블이 나온 사람이 나라면
        {
            //if (diceThrower == PlayerType.Human)
            //{
            //    Debug.Log($"{diceThrower}이 더블이 나왔습니다.");
            //}

            if (islandWaitTime > 0)
            {
                islandWaitTime = 0; // 무인도에 있는 상황이면 무인도 탈출
            }
            else
            {
                actionCount++;      // 일반 상황이면 한번 더 던지기
            }
        }
    }

    /// <summary>
    /// 주사위를 굴리는 함수
    /// </summary>
    public void RollDice()
    {
        // 행동력이 있을 때만 실행
        if (actionCount > 0)
        {
            actionCount--;

            //// 주사위 돌리는 애니메이션 등 처리
            //Debug.Log($"{Type}은 {dicesum}이 나왔습니다.");
            //string str = $"{Type}은 {(int)this.Position}에서 ";
            
            if (islandWaitTime <= 0)    // 무인도가 아닌 상황
            {
                int dicesum = dice.RollAll_GetTotalSum(Type == PlayerType.Human);   // 주사위 굴리고
                map.Move(this, dicesum);    // 이동시키기
                //map.Move(this, 10);
                //str += $"{(int)this.Position}에 도착했습니다.";
                //Debug.Log(str);
                GameManager.Inst.UI_Manager.SetResultText(Type, dicesum);   // 결과를 결과창에 띄우기
            }
            else
            {
                // 무인도인 상황
                dice.RollAll_GetTotalSum(Type == PlayerType.Human);
                if (islandWaitTime > 0) // 더블이 안나온 상황
                {
                    if (Type == PlayerType.Human)
                    {
                        Debug.Log($"{Type} 무인도 탈출 실패");
                    }
                    GameManager.Inst.UI_Manager.SetResultText($"{Type} 무인도 탈출 실패");
                }
            }
        }
    }
}
