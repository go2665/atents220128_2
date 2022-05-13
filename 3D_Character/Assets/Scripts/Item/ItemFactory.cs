using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    static int itemCount = 0;

    static public GameObject GetItem(uint id)
    {
        return GetItem((ItemIDCode)id);
    }

    static public GameObject GetItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();
        Item item = obj.AddComponent<Item>();
        //switch (code)
        //{
        //    case ItemIDCode.DUMMY:
        //        item = obj.AddComponent<Item>();                
        //        break;
        //    case ItemIDCode.HealthPotion:
        //        item = obj.AddComponent<HeathPotion>();
        //        break;
        //    case ItemIDCode.ManaPotion:
        //        item = obj.AddComponent<ManaPotion>();
        //        break;
        //    case ItemIDCode.GoldCoin:
        //        item = obj.AddComponent<GoldCoin>();
        //        break;
        //    default:
        //        break;
        //}
        item.data = GameManager.Inst.ItemDatas[code];

        string[] itemName = item.data.name.Split("_");
        obj.name = $"{itemName[1]}_{itemCount}";
        itemCount++;

        return obj;
    }
}
