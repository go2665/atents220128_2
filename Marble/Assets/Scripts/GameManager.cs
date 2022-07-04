using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    DiceSet diceSet;
    public DiceSet GameDiceSet
    {
        get => diceSet;
    }

    Player[] players = null;
    public Player[] Players
    {
        get => players;
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
