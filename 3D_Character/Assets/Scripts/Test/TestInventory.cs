using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{   
    void Start()
    {
        Inventory inven = new Inventory(8);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        //inven.RemoveItem(0);
        //inven.RemoveItem(5);
        //inven.RemoveItem(0);
        inven.Test_PrintInventory();
        inven.MoveItem(0, 4);
        inven.Test_PrintInventory();
        inven.MoveItem(2, 0);
        inven.Test_PrintInventory();
        inven.MoveItem(7, 0);
        inven.MoveItem(0, 7);
        inven.Test_PrintInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
