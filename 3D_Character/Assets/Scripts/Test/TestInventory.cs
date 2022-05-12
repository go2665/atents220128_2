using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{   
    void Start()
    {
        //Test1_AddRemove();
        //Test2_Move();
        //Test3_Clear();
    }

    private static void Test3_Clear()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.Test_PrintInventory();
        inven.ClearInventory();
        inven.Test_PrintInventory();
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealthPotion]);
        inven.Test_PrintInventory();
    }

    private static void Test2_Move()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.Test_PrintInventory();
        inven.MoveItem(0, 4);
        inven.Test_PrintInventory();
        inven.MoveItem(2, 0);
        inven.Test_PrintInventory();
        inven.MoveItem(7, 0);
        inven.MoveItem(0, 7);
        inven.Test_PrintInventory();
    }

    void Test1_AddRemove()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.RemoveItem(0);
        inven.RemoveItem(8);
        inven.RemoveItem(0);
    }
}
