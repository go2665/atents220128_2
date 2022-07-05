using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const int StartMoney = 5000;

    MapID position = MapID.Start;
    int money;
    PlayerType type = PlayerType.Bank;

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

    public void Initialize(PlayerType playerType)
    {
        type = playerType;
        Money = StartMoney;
    }

    public void OnArriveIsland(int wait)
    {
        islandWaitTime = wait;
    }
}
