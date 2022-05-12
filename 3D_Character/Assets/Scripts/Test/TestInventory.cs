using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{   
    void Start()
    {
        Inventory inven = new Inventory(2);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.HealthPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.ManaPotion]);
        inven.AddItem(GameManager.Inst.ItemDatas[ItemIDCode.GoldCoin]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
