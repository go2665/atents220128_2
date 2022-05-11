using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player mainPlayer = null;
    public Player MainPlayer
    {
        get
        {
            return mainPlayer;
        }
    }

    private ItemDataManager itemDatas = null;
    public ItemDataManager ItemDatas
    {
        get
        {
            return itemDatas;
        }
    }

    private static GameManager instance = null;
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
            DontDestroyOnLoad(this.gameObject);

            instance.Initialize();
        }
        else
        {
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        mainPlayer = FindObjectOfType<Player>();
        itemDatas = GetComponent<ItemDataManager>();
        itemDatas.Initialize();
    }
}
