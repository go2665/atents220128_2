using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BattleField leftField = null;
    BattleField rightField = null;
    public BattleField LeftField
    {
        get => leftField;
    }
    public BattleField RightField
    {
        get => rightField;
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
        GameObject field = GameObject.FindGameObjectWithTag("PlayerField");
        leftField = field.GetComponent<BattleField>();
        field = GameObject.FindGameObjectWithTag("EnemyField");
        rightField = field.GetComponent<BattleField>();
    }
}
