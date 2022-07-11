using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    DiceSet dice;
    Map map;

    const int StartMoney = 5000;

    MapID position = MapID.Start;
    int money;
    PlayerType type = PlayerType.Bank;
    Material material;
    int actionCount = 1;


    int islandWaitTime = -1; 

    public int Money 
    { 
        get => money; 
        set
        {
            money = value;
            OnMoneyChange?.Invoke(money);
        }
    }

    public MapID Position
    {
        get => position;
        set
        {
            position = value;
        }
    }

    public PlayerType Type
    {
        get => type;
    }

    public bool ActionDone
    {
        get => actionCount < 1;
    }


    public System.Action<int> OnMoneyChange;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    public void Initialize(PlayerType playerType)
    {
        type = playerType;
        Color color = GameManager.Inst.PlayerColor[(int)type];
        material.color = color;
        if (type == PlayerType.Bank)
        {
            Money = 2000000000;
            gameObject.SetActive(false);
        }
        else
        {
            Money = StartMoney;  
        }

        gameObject.name = $"Player_{playerType}";

        dice = GameManager.Inst.GameDiceSet;
        map = GameManager.Inst.GameMap;

        GameManager.Inst.TurnManager.OnTurnEnd += OnTurnEnd;
        GameManager.Inst.GameDiceSet.OnDouble += OnDouble;
    }

    public void OnArriveIsland(int wait)
    {
        islandWaitTime = wait;
    }

    public void OnTurnEnd()
    {
        if (type == PlayerType.Human)
        {
            Debug.Log($"{type} turn end");
        }
        actionCount = 1;
        islandWaitTime--;
    }

    void OnDouble(PlayerType diceThrower)
    {
        if (diceThrower == type)
        {
            if (diceThrower == PlayerType.Human)
            {
                Debug.Log($"{diceThrower}이 더블이 나왔습니다.");
            }

            if (islandWaitTime > 0)
            {
                islandWaitTime = 0;
            }
            else
            {
                actionCount++;
            }
        }
    }

    public void RollDice()
    {
        if (actionCount > 0)
        {
            actionCount--;

            // 주사위 돌리는 애니메이션 등 처리
            //Debug.Log($"{Type}은 {dicesum}이 나왔습니다.");
            //string str = $"{Type}은 {(int)this.Position}에서 ";
            if (islandWaitTime <= 0)
            {
                int dicesum = dice.RollAll_GetTotalSum(Type == PlayerType.Human);
                map.Move(this, dicesum);
                //map.Move(this, 10);
                //str += $"{(int)this.Position}에 도착했습니다.";
                //Debug.Log(str);
                GameManager.Inst.UI_Manager.SetResultText(Type, dicesum);
            }
            else
            {
                dice.RollAll_GetTotalSum(Type == PlayerType.Human);
                if (Type == PlayerType.Human)
                {
                    Debug.Log($"{Type} 무인도 탈출 실패");
                }
                GameManager.Inst.UI_Manager.SetResultText($"{Type} 무인도 탈출 실패");
            }
        }
    }
}
