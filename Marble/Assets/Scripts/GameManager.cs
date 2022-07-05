using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    readonly public int NumOfPlayer = System.Enum.GetValues(typeof(PlayerType)).Length;

    DiceSet diceSet;
    public DiceSet GameDiceSet
    {
        get => diceSet;
    }

    Player[] players = null;    // 은행 포함
    public Player[] Players
    {
        get => players;
    }

    Map map = null;
    public Map GameMap
    {
        get => map;
    }

    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 초기화 함수.
    /// </summary>
    void Initialize()
    {
        Debug.Log("Game Manager Initialize");
        diceSet = FindObjectOfType<DiceSet>();
    }
}
