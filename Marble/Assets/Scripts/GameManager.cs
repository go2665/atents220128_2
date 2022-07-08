using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject playerPrefab;
    readonly public int NumOfPlayer = System.Enum.GetValues(typeof(PlayerType)).Length;
    readonly public Color[] PlayerColor = new Color[] { Color.black,
        new(0, 0.1726165f, 0.6764706f, 0.454902f), new(0.7352941f, 0,0, 0.454902f), 
        new(0.7352941f, 0.6237322f, 0, 0.454902f), new(0, 0, 0, 0.454902f) };

    TurnManager turnManager;
    public TurnManager TurnManager
    {
        get => turnManager;
    }

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
        map = FindObjectOfType<Map>();
         

        players = new Player[NumOfPlayer];
        for(int i=0; i<NumOfPlayer; i++ )
        {
            players[i] = Instantiate(playerPrefab, transform).GetComponent<Player>();
            players[i].Initialize((PlayerType)i);

            if (i != 0)
            {
                map.Move(players[i], MapID.Start);
            }
        }

        turnManager = FindObjectOfType<TurnManager>();
        turnManager.Initialize();

    }

    public Player GetPlayer(PlayerType type)
    {
        return Players[(int)type];
    }

    public void PlayerRollDice(Player player)
    {
        int roll = diceSet.RollAll_GetTotalSum();
    }
}
