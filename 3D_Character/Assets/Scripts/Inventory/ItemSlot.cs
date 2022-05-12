using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    private ItemData slotItem = null;
    /// <summary>
    /// 슬롯이 가진 아이템 데이터. 읽기전용
    /// </summary>
    public ItemData SlotItem
    {
        get => slotItem;
        //get
        //{
        //    return slotItem;
        //}
        //set => slotItem = value; //예시용
    }

    /// <summary>
    /// 슬롯에 아이템을 설정할 때 사용
    /// </summary>
    /// <param name="itemData">슬롯에 설정할 아이템</param>
    public void AssignSlotItem(ItemData itemData)
    {
        slotItem = itemData;
    }

    /// <summary>
    /// 슬롯에 아이템을 비울 때 사용
    /// </summary>
    public void ReleaseSlotItem()
    {
        slotItem = null;
    }
}
