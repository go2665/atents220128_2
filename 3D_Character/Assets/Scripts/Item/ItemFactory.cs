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

    /// <summary>
    /// 여러 아이템을 한번에 생성하는 함수. position에서 반경 0.5안의 범위에서 랜덤으로 위치가 결정된다.
    /// </summary>
    /// <param name="code">생성할 아이템의 종류</param>
    /// <param name="position">아이템이 생성될 기준 위치</param>
    /// <param name="count">생성할 아이템의 개수</param>
    static public void GetItems(ItemIDCode code, Vector3 position, uint count)
    {        
        for (int i = 0; i < count; i++)
        {
            GameObject obj = GetItem(code);
            Vector2 noise = Random.insideUnitCircle * 0.5f;
            position.x += noise.x;
            position.z += noise.y;
            obj.transform.position = position;
        }
    }

    /// <summary>
    /// 여러 아이템을 한번에 생성하는 함수. position에서 반경 0.5안의 범위에서 랜덤으로 위치가 결정된다.
    /// </summary>
    /// <param name="id">생성할 아이템의 아이디</param>
    /// <param name="position">아이템이 생성될 기준 위치</param>
    /// <param name="count">생성할 아이템의 개수</param>
    static public void GetItems(uint id, Vector3 position, uint count)
    {
        GetItems((ItemIDCode)id, position, count);
    }
}
