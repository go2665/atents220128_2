using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어 말과 Player 스크립트가 들어있는 프리팹
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// 전체 플레이어의 수(은행이 포함되어서 5)
    /// </summary>
    readonly public int NumOfPlayer = System.Enum.GetValues(typeof(PlayerType)).Length;

    /// <summary>
    /// 플레이어의 색상
    /// </summary>
    readonly public Color[] PlayerColor = new Color[] { Color.black,
        new(0, 0.1726165f, 0.6764706f, 0.454902f), new(0.7352941f, 0,0, 0.454902f), 
        new(0.7352941f, 0.7352941f, 0.7352941f, 0.454902f), new(0, 0, 0, 0.454902f) };

    /// <summary>
    /// UI 매니저
    /// </summary>
    UI_Manager ui_Manager;
    public UI_Manager UI_Manager
    {
        get => ui_Manager;
    }

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;
    public TurnManager TurnManager
    {
        get => turnManager;
    }

    /// <summary>
    /// 주사위
    /// </summary>
    DiceSet diceSet;
    public DiceSet GameDiceSet
    {
        get => diceSet;
    }

    /// <summary>
    /// 게임에 참여한 플레이어(은행포함)
    /// </summary>
    Player[] players = null;
    public Player[] Players
    {
        get => players;
    }

    /// <summary>
    /// 게임 판(플레이어들 이동처리)
    /// </summary>
    Map map = null;
    public Map GameMap
    {
        get => map;
    }

    GoldenKeyManager goldenKeyManager;
    public GoldenKeyManager GoldenKeyManager
    {
        get => goldenKeyManager;
    }

    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 초기화 함수. 모든 메니저들을 순서에 맞게 초기화 실행
    /// 주사위->맵->플레이어->UI매니저->턴매니저
    /// </summary>
    void Initialize()
    {
        Debug.Log("Game Manager Initialize");
        
        diceSet = GetComponent<DiceSet>();

        map = FindObjectOfType<Map>();
        map.Initialize();

        players = new Player[NumOfPlayer];
        for(int i=0; i<NumOfPlayer; i++ )
        {
            players[i] = Instantiate(playerPrefab, transform).GetComponent<Player>();
            players[i].Initialize((PlayerType)i);

            if (i != 0)
            {
                players[i].SetPosition(MapID.Start);
            }
        }        

        ui_Manager = GetComponent<UI_Manager>();
        ui_Manager.Initialize();

        turnManager = GetComponent<TurnManager>();
        turnManager.Initialize();

        goldenKeyManager = GetComponent<GoldenKeyManager>();
        goldenKeyManager.Initialize();
    }

    private void Start()
    {
        // 턴 시작
        TurnManager.GameStart();
    }

    /// <summary>
    /// 특정 플레이어 리턴
    /// </summary>
    /// <param name="type">리턴할 플레이어</param>
    /// <returns>플레이어</returns>
    public Player GetPlayer(PlayerType type)
    {
        return Players[(int)type];
    }
}
