using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 배의 프리팹
    /// </summary>
    public GameObject[] ships = new GameObject[(int)ShipType.SizeOfShipType];

    /// <summary>
    /// 포탄의 프리팹
    /// </summary>
    public GameObject bombMark_Success = null;
    public GameObject bombMark_Fail = null;

    Player playerLeft = null;
    Player playerRight = null;
    BattleField fieldLeft = null;
    BattleField fieldRight = null;

    TurnManager turnManager = null;

    GameState state = GameState.Ready;

    public GameState State
    {
        get => state;
    }

    public Player PlayerLeft
    {
        get => playerLeft;
    }
    public Player PlayerRight
    {
        get => playerRight;
    }
    public BattleField FieldLeft
    {
        get => fieldLeft;
    }
    public BattleField FieldRight
    {
        get => fieldRight;
    }

    static GameManager instance = null;
    public static GameManager Inst
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    void Initialize()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     // 플레이어 찾기
        playerLeft = player.GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Enemy");                 // 적 찾기
        playerRight = player.GetComponent<Player>();

        GameObject field = GameObject.FindGameObjectWithTag("PlayerField"); // 아군 필드 찾기
        fieldLeft = field.GetComponent<BattleField>();
        fieldLeft.Initialize(true);
        field = GameObject.FindGameObjectWithTag("EnemyField");             // 적 필드 찾기
        fieldRight = field.GetComponent<BattleField>();
        fieldRight.Initialize();

        playerLeft.Initialize(fieldLeft, fieldRight);
        playerRight.Initialize(fieldRight, fieldLeft);

        turnManager = GetComponentInChildren<TurnManager>();
        turnManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// 함선 생성
    /// </summary>
    /// <param name="shipType">생성할 함수의 타입</param>
    /// <returns>생성한 배</returns>
    public Ship MakeShip(ShipType shipType)
    {
        return Instantiate(ships[(int)shipType]).GetComponent<Ship>();
    }

    /// <summary>
    /// 폭탄 표시 생성
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    public GameObject MakeBombMark(bool isSuccess)
    {
        GameObject obj;
        if (isSuccess)
        {
            obj = Instantiate(bombMark_Success);
        }
        else
        {
            obj = Instantiate(bombMark_Fail);
        }
        return obj;
    }


    /// <summary>
    /// 상태 설정용 함수
    /// </summary>
    /// <param name="newState">새 상태</param>
    public void StateChange(GameState newState)
    {
        // 이전 상태가 끝나며 할 일
        switch (state)
        {
            case GameState.Ready: 
                break;
            case GameState.ShipDeployment:
                break;
            case GameState.Battle:
                turnManager.gameObject.SetActive(false);
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }

        // 새로운 상태로 들어가며 해야 할 일
        switch (newState)
        {
            case GameState.Ready:   // 할 일 없음                
                break;
            case GameState.ShipDeployment:
                break;
            case GameState.Battle:
                turnManager.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
        state = newState;
    }

    public GameObject GetCurrentCanvas()
    {
        return FindObjectOfType<Canvas_Battle>().gameObject;
    }
}
