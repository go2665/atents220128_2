using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const int StartMoney = 5000;

    MapID position = MapID.Start;
    int money;
    PlayerType type = PlayerType.Bank;
    Material material;
    int actionCount = 0;

    int islandWaitTime = 0; 

    public int Money 
    { 
        get => money; 
        set
        {
            money = value;
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

        //GameManager.Inst.TurnManager.OnTurnEnd += OnTurnEnd;
        //GameManager.Inst.GameDiceSet.OnDouble += OnDouble;
    }

    private void OnDestroy()
    {
        //GameManager.Inst.GameDiceSet.OnDouble -= OnDouble;
        //GameManager.Inst.TurnManager.OnTurnEnd -= OnTurnEnd;
    }

    public void OnArriveIsland(int wait)
    {
        islandWaitTime = wait;
    }

    void OnTurnEnd()
    {
        actionCount = 1;
    }

    void OnDouble(PlayerType diceThrower)
    {
        if (diceThrower == type)
        {
            actionCount++;
        }
    }

    public void RollDice()
    {
        actionCount--;
        // 주사위 돌리는 애니메이션 등 처리
        // 입력이 들어오면 DiceSet 클래스 사용
    }
}
