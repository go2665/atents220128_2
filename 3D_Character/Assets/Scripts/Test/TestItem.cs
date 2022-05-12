using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public ItemIDCode itemID = ItemIDCode.DUMMY;
    // Start is called before the first frame update
    void Start()
    {
        //ItemData item = GameManager.Inst.ItemDatas[0];
        GameObject obj = ItemFactory.GetItem(itemID);
        obj.transform.position = this.transform.position;


        Inventory inven = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
