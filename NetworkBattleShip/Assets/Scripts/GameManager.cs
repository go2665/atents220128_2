using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 배의 프리팹
    /// </summary>
    public GameObject[] ships = new GameObject[(int)ShipType.SizeOfShipType];

    /// <summary>
    /// 포탄의 프리팹들
    /// </summary>
    public GameObject bombMark_Success = null;
    public GameObject bombMark_Fail = null;

    Dictionary<GameState, GameObject> canvases = new();

    TextMeshProUGUI debugInfo = null;


    NetPlayer playerLeft = null;
    NetPlayer playerRight = null;
    BattleField fieldLeft = null;
    BattleField fieldRight = null;

    TurnManager turnManager = null;     // 턴 매니저

    GameState state = GameState.Ready;

    public GameState State
    {
        get => state;
    }

    public NetPlayer PlayerLeft
    {
        get => playerLeft;
    }
    public NetPlayer PlayerRight
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

    public TurnManager TurnManager
    {
        get => turnManager;
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
        GameObject field = GameObject.FindGameObjectWithTag("PlayerField"); // 아군 필드 찾기
        fieldLeft = field.GetComponent<BattleField>();
        fieldLeft.Initialize(true);
        field = GameObject.FindGameObjectWithTag("EnemyField");             // 적 필드 찾기
        fieldRight = field.GetComponent<BattleField>();
        fieldRight.Initialize();        

        turnManager = GetComponentInChildren<TurnManager>();
        turnManager.gameObject.SetActive(false);

        canvases[GameState.Ready] = FindObjectOfType<Canvas_Ready>().gameObject;
        canvases[GameState.ShipDeployment] = FindObjectOfType<Canvas_Deployment>().gameObject;
        canvases[GameState.Battle] = FindObjectOfType<Canvas_Battle>().gameObject;
        canvases[GameState.GameOver] = FindObjectOfType<Canvas_Result>().gameObject;
        foreach (var canvasPair in canvases)
        {
            canvasPair.Value.SetActive(false);
        }

        debugInfo = GameObject.Find("DebugInfo").GetComponent<TextMeshProUGUI>();

        StateChange(GameState.Ready);
    }

    public void PrintDebugInfo(string str)
    {
        debugInfo.text = str;
    }

    public void PlayerConnected(bool isPlayer, NetPlayer player)
    {        
        if(isPlayer)
        {
            playerLeft = player;
        }
        else
        {
            playerRight = player;
        }
        //player = GameObject.FindGameObjectWithTag("Enemy");                 // 적 찾기
        //playerRight = player.GetComponent<Player>();
        //playerLeft.Initialize(fieldLeft, fieldRight);
        //playerRight.Initialize(fieldRight, fieldLeft);
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
        canvases[state].SetActive(false);
        switch (state)
        {
            case GameState.ShipDeployment:
                FieldRight.RandomDeployment();
                break;
            case GameState.Battle:
                turnManager.gameObject.SetActive(false);
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }

        canvases[newState].SetActive(true);
        // 새로운 상태로 들어가며 해야 할 일
        switch (newState)
        {
            case GameState.ShipDeployment:                
                break;
            case GameState.Battle:
                //turnManager.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                Canvas_Result canvasResult = canvases[newState].GetComponent<Canvas_Result>();
                if (!PlayerLeft.IsDepeat)
                {
                    Debug.Log("Player Win");
                    canvasResult.OnVictory();
                }
                else
                {
                    Debug.Log("Enemy Win");
                    canvasResult.OnDefeat();
                }
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
