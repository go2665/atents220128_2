using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    private int itemCount = 0;                  // 이 인벤토리 칸에 들어있는 아이템 개수(같은 종류일 때만)
    public int ItemCount { get => itemCount; }

    private bool itemEquiped = false;
    public bool ItemEquiped { get => itemEquiped; }

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
    public void AssignSlotItem(ItemData itemData, int count = 1)
    {
        slotItem = itemData;
        itemCount = count;
        onSlotItemChange?.Invoke();     // 실제로 슬롯에 아이템이 변경되었을 때 델리게이트 실행
    }

    /// <summary>
    /// 슬롯에 아이템을 비울 때 사용
    /// </summary>
    public void ReleaseSlotItem()
    {
        slotItem = null;
        itemCount = 0;
        onSlotItemChange?.Invoke();     // 실제로 슬롯에 아이템이 변경되었을 때 델리게이트 실행
    }

    /// <summary>
    /// 같은 종류의 아이템을 추가해서 아이템 갯수가 증가하는 상황에 사용
    /// </summary>
    public void IncreaseSlotItem(int count = 1)
    {
        itemCount += count;             // 갯수 증가시킴
        onSlotItemChange?.Invoke();     // 슬롯 UI를 리프레쉬
    }

    /// <summary>
    /// 같은 종류의 아이템을 삭제해서 아이템 갯수가 감소하는 상황에 사용
    /// </summary>
    public void DecreaseSlotItem(int count = 1)
    {
        itemCount -= count;             // 갯수 감소시킴
        onSlotItemChange?.Invoke();     // 슬롯 UI를 리프레쉬
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템을 사용
    /// </summary>
    /// <param name="target">아이템의 효과를 적용받을 대상</param>
    public void UseSlot(GameObject target = null)
    {
        // 슬롯에 아이템이 있을 때만 사용 시도
        if( slotItem != null )
        {
            IUseableItem useable = slotItem as IUseableItem;    // 슬롯에 들어있는 아이템이 사용 가능한지 확인
            if (useable != null)
            {
                if (itemCount > 1)      // 개수가 2개 이상일 때 
                {
                    DecreaseSlotItem();
                }
                else
                {
                    ReleaseSlotItem();  // 0개가 남으므로 슬롯 비우기
                }
                useable.Use(target);    // 사용 가능한 아이템이면 사용
            }

            IEquipableItem equipable = slotItem as IEquipableItem;
            if( equipable != null )
            {
                itemEquiped = equipable.ToggleEquipItem();
                onSlotItemChange?.Invoke();
            }
        }
    }
}
