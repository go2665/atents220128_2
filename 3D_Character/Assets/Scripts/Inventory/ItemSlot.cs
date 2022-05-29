using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // 델리게이트들---------------------------------------------------------------------------------
    /// <summary>
    /// 델리게이트 타입이랑 변수 만들기
    /// </summary>
    public delegate void SlotChangeDelegate();
    public SlotChangeDelegate onSlotItemChange = null;
    public SlotChangeDelegate onEquipSlotChange = null;


    // 변수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 이 인벤토리 칸에 들어있는 아이템 개수.
    /// </summary>
    private uint itemCount = 0;

    /// <summary>
    /// 슬롯이 가진 아이템 데이터.
    /// </summary>
    private ItemData slotItemData = null;

    /// <summary>
    /// 슬롯의 아이템을 장비했는지 여부
    /// </summary>
    private bool itemEquiped = false;


    // 프로퍼티들-----------------------------------------------------------------------------------
    /// <summary>
    /// 이 인벤토리 칸에 들어있는 아이템 개수. 읽기전용 프로퍼티
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            itemCount = value;
            if(itemCount == 0)
            {
                slotItemData = null;
            }
            onSlotItemChange?.Invoke(); // 실제로 슬롯에 아이템이 변경되거나 개수가 변경되었을 때 델리게이트 실행
        }
    }
    
    /// <summary>
    /// 슬롯이 가진 아이템 데이터. 읽기전용 프로퍼티
    /// </summary>
    public ItemData SlotItemData
    {
        get => slotItemData;
    }

    /// <summary>
    /// 슬롯의 아이템을 장비했는지 여부. 읽기 전용.
    /// </summary>
    public bool ItemEquiped
    {
        get => itemEquiped;
        set
        {
            itemEquiped = value;

            if(itemEquiped)
            {
                if( equiped != null && equiped != this)
                {
                    equiped.ItemEquiped = false;
                }
                equiped = this;
            }
            onEquipSlotChange?.Invoke(); // 값이 변경되면 델리게이트 실행
        }
    }

    static ItemSlot equiped = null; // 임시방편

    // 함수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 슬롯에 아이템을 설정할 때 사용
    /// </summary>
    /// <param name="itemData">슬롯에 설정할 아이템</param>
    /// <param name="count">슬롯에 들어갈 아이템 개수</param>
    public void AssignSlotItem(ItemData itemData, uint count = 1)
    {
        slotItemData = itemData;
        ItemCount = count;
        itemEquiped = false;
    }

    /// <summary>
    /// 슬롯에 아이템을 비울 때 사용
    /// </summary>
    public void ClearSlotItem()
    {
        ItemCount = 0;  //slotItemData는 프로퍼티가 비움
    }
        
    /// <summary>
    /// 같은 종류의 아이템을 추가해서 아이템 갯수가 증가하는 상황에 사용
    /// </summary>
    /// <param name="count">증가시킬 개수. 기본값 1</param>
    /// <returns>추가할 때 최대치를 넘어 넘친 갯수. 0이면 정상</returns>
    public uint IncreaseSlotItem(uint count = 1)
    {
        uint newCount = ItemCount + count;
        int overCount = (int)newCount - (int)SlotItemData.maxStackCount;
        if( overCount > 0 )
        {
            // 증가했더니 넘쳤다.
            ItemCount = SlotItemData.maxStackCount;
        }
        else
        {
            // 충분히 증가시킬 수 있다.
            ItemCount = newCount;
            overCount = 0;  // uint 캐스팅을 위해 음수 제거
        }
        return (uint)overCount;
    }

    
    /// <summary>
    /// 같은 종류의 아이템을 삭제해서 아이템 개수가 감소하는 상황에 사용
    /// </summary>
    /// <param name="count">감소시킬 개수. 기본값 1</param>
    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;        
        if(newCount < 1)
        {
            ClearSlotItem();
        }
        else
        {
            ItemCount = (uint)newCount;            
        }        
    }
    
    /// <summary>
    /// 슬롯에 들어있는 아이템을 사용하거나 착용
    /// </summary>
    /// <param name="target">아이템의 효과를 적용받거나 아이템을 착용할 대상</param>
    public void UseSlot(GameObject target = null)
    {
        // 슬롯에 아이템이 있을 때만 사용 시도
        if( slotItemData != null )
        {
            UseUseableItem(target);
            UseEquippableItem(target);
        }
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템을 장비 또는 해제
    /// </summary>
    /// <param name="target">아이템을 착용하거나 해제할 대상</param>
    private void UseEquippableItem(GameObject target)
    {
        IEquippableItem equipableItem = slotItemData as IEquippableItem;  // 슬롯에 들어있는 아이템이 장착 가능한지 확인
        if (equipableItem != null)
        {
            bool equip = false;     // 이번에 아이템을 착용했는지 체크

            // 장착 가능한 아이템이다.
            IEquippableCharacter player = target.GetComponent<IEquippableCharacter>();
            if (player.IsEquipWeapon())
            {
                // 아이템을 장착 중이다.
                if (player.EquipItem != slotItemData)
                {
                    equipableItem.UnEquipItem(player);  // 일단 장착해 놓은 아이템 해제하고
                    equipableItem.EquipItem(player);    // 다른 종류의 아이템을 플레이어에 장착
                    equip = true;
                }
                else
                {
                    equipableItem.UnEquipItem(player);  // 같은 종류의 아이템이면 플레이어에게서 장착 해제
                }
            }
            else
            {
                // 아이템을 장착하고 있지 않다.
                equipableItem.EquipItem(player);        // 플레이어에게 장착
                equip = true;
            }

            ItemEquiped = equip;    // ItemEquiped에 set을 한번만 해주기 위해
        }
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템을 사용
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    private void UseUseableItem(GameObject target)
    {
        IUseableItem useable = slotItemData as IUseableItem;    // 슬롯에 들어있는 아이템이 사용 가능한지 확인
        if (useable != null)
        {
            useable.Use(target);    // 사용 가능한 아이템이면 사용
            DecreaseSlotItem();     // 아이템 개수 1개 감소
        }
    }

}
