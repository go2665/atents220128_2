using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 배의 프리팹
    /// </summary>
    public GameObject[] ships = new GameObject[(int)ShipType.SizeOfShipType];

    Player playerLeft = null;
    Player playerRight = null;
    BattleField fieldLeft = null;
    BattleField fieldRight = null;

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
        field = GameObject.FindGameObjectWithTag("EnemyField");             // 적 필드 찾기
        fieldRight = field.GetComponent<BattleField>();
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
}
