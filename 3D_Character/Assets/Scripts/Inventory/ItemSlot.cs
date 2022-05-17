using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    /// <summary>
    /// 델리게이트 타입이랑 변수 만들기
    /// </summary>
    public delegate void SlotChangeDelegate();
    public SlotChangeDelegate onSlotItemChange = null;

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
        onSlotItemChange?.Invoke();     // 실제로 슬롯에 아이템이 변경되었을 때 델리게이트 실행
    }

    /// <summary>
    /// 슬롯에 아이템을 비울 때 사용
    /// </summary>
    public void ReleaseSlotItem()
    {
        slotItem = null;
        onSlotItemChange?.Invoke();     // 실제로 슬롯에 아이템이 변경되었을 때 델리게이트 실행
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템을 사용
    /// </summary>
    /// <param name="target">아이템의 효과를 적용받을 대상</param>
    public void UseItem(GameObject target = null)
    {
        // 슬롯에 아이템이 있을 때만 사용 시도
        if( slotItem != null )
        {
            IUseableItem useable = slotItem as IUseableItem;    // 슬롯에 들어있는 아이템이 사용 가능한지 확인
            if( useable != null)    
            {
                useable.Use(target);    // 사용 가능한 아이템이면 사용
                ReleaseSlotItem();      // 슬롯 비우기
            }
        }
    }
}
