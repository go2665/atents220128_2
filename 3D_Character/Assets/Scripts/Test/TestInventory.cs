using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInventory : MonoBehaviour
{
    void Start()
    {
        //Test1_AddRemove();
        //Test2_Move();
        //Test3_Clear();
        //Test4_ItemUse();
        //Test5_InvenUI();
        //Test6_PlayerInvenUI();
        Test7_Inventory();
    }

    private void Update()
    {        
        if( Keyboard.current.zKey.wasPressedThisFrame )
        {
            //GameManager.Inst.MainPlayer.Inven.MoveItem(0, 1);
            ItemFactory.GetItems(ItemIDCode.HealingPotion, GameManager.Inst.MainPlayer.transform.position, 3);
        }
    }

    private void Test7_Inventory()
    {
        // 힐포5, 금화2, 마포3, 칼, 테스트칼
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.Sword]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.Sword_Test]);

        GameManager.Inst.MainPlayer.Inven.SplitItem(0, 1, 1);
        GameManager.Inst.MainPlayer.Inven.SplitItem(0, 6, 2);
        GameManager.Inst.MainPlayer.Inven.SplitItem(1, 7, 2);

        GameManager.Inst.MainPlayer.onInventoryOnOff();
    }

    private static void Test6_PlayerInvenUI()
    {
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.Sword]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.Sword_Test]);
        GameManager.Inst.MainPlayer.Inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.Sword_Test]);
        //GameManager.Inst.MainPlayer.Inven.Test_PrintInventory(); 
    }

    private static void Test5_InvenUI()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        InventoryUI invenUI = GameObject.FindObjectOfType<InventoryUI>();
        invenUI.InitializeInventory(inven);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);        
    }

    /// <summary>
    /// 아이템 사용 테스트
    /// </summary>
    private static void Test4_ItemUse()
    {        
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealingPotion]);
        inven.Test_PrintInventory();
        ItemSlot slot = inven.GetSlot(0);
        //ItemData_HealingPotion potion = slot.SlotItem as ItemData_HealingPotion;    // ItemData_HealingPotion로 캐스팅 할 수 있으면 진행하고 못하면 null을 할당한다.
        //ItemData_HealingPotion potion2 = (ItemData_HealingPotion)slot.SlotItem;        
        slot?.UseSlot();
        inven.Test_PrintInventory();
    }

    /// <summary>
    /// 인벤토리 비우기 테스트
    /// </summary>
    private static void Test3_Clear()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeHealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.Test_PrintInventory();
        inven.ClearInventory();
        inven.Test_PrintInventory();
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeHealthPotion]);
        inven.Test_PrintInventory();
    }

    /// <summary>
    /// 인벤토리에서 아이템 위치 바꾸기 테스트
    /// </summary>
    private static void Test2_Move()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeHealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.Test_PrintInventory();
        inven.MoveItem(0, 4);
        inven.Test_PrintInventory();
        inven.MoveItem(2, 0);
        inven.Test_PrintInventory();
        inven.MoveItem(7, 0);
        inven.MoveItem(0, 7);
        inven.Test_PrintInventory();
    }

    /// <summary>
    /// 인벤토리에 아이템 추가 삭제 테스트
    /// </summary>
    void Test1_AddRemove()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeHealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.FakeManaPotion]);
        inven.ClearSlot(0);
        inven.ClearSlot(8);
        inven.ClearSlot(0);
    }
}
