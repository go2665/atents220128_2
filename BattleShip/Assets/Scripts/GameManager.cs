using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    void Initialize()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerLeft = player.GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Enemy");
        playerRight = player.GetComponent<Player>();

        GameObject field = GameObject.FindGameObjectWithTag("PlayerField");
        fieldLeft = field.GetComponent<BattleField>();
        field = GameObject.FindGameObjectWithTag("EnemyField");
        fieldRight = field.GetComponent<BattleField>();
    }

    public Ship MakeShip(ShipType shipType)
    {
        return Instantiate(ships[(int)shipType]).GetComponent<Ship>();
    }
}
