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

    // 인벤토리 UI에 접근 가능하도록 변수와 프로퍼티 작성
    private InventoryUI inventoryUI = null; 
    public InventoryUI InventoryUI
    {
        get => inventoryUI;
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
        itemDatas = GetComponent<ItemDataManager>();
        itemDatas.Initialize();

        mainPlayer = FindObjectOfType<Player>();        // 문제의 소지가 있음
        inventoryUI = FindObjectOfType<InventoryUI>();  // 문제의 소지가 있음
    }
}
