using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{    
    public GameObject[] prefabs = null;

    public GameObject GetItem(uint id)
    {
        //Instantiate(prefabs[id]);
        return GetItem((ItemIDCode)id);
    }

    public GameObject GetItem(ItemIDCode code)
    {
        //GameObject obj = Instantiate(prefabs[(int)code]);
        GameObject obj = new GameObject();
        Item item = null;
        switch (code)
        {
            case ItemIDCode.DUMMY:
                item = obj.AddComponent<Item>();                
                break;
            case ItemIDCode.HealthPotion:
                item = obj.AddComponent<HeathPotion>();
                break;
            case ItemIDCode.ManaPotion:
                item = obj.AddComponent<ManaPotion>();
                break;
            case ItemIDCode.GoldCoin:
                item = obj.AddComponent<GoldCoin>();
                break;
            default:
                break;
        }
        item.data = GameManager.Inst.ItemDatas[code];

        return null;
    }
}
