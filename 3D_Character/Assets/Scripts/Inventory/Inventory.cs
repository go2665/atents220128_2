using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    // 변수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 아이템 슬롯(아이템 칸)의 배열
    /// </summary>
    ItemSlot[] slots = null;

    ItemSlot spliteTempSlot = null;


    // 상수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 상수 : 기본 아이템 슬롯 수
    /// </summary>
    const int DEFAULT_INVENTORY_SIZE = 5;


    // 프로퍼티들-----------------------------------------------------------------------------------
    /// <summary>
    /// 현재 아이템 슬롯 수
    /// </summary>
    public int SlotCount { get => slots.Length; }
    public ItemSlot SpliteTempSlot { get => spliteTempSlot; }


    // 함수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 인벤토리에 들어갈 슬롯과 빈슬롯 큐 초기화
    /// </summary>
    /// <param name="size">인벤토리의 칸수. 기본값 5. </param>
    public Inventory(int size = DEFAULT_INVENTORY_SIZE) //생성자
    {
        slots = new ItemSlot[size];                     // size에 지정된 숫자만큼 슬롯을 만든다.
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot();                  // 슬롯 생성
        }
        spliteTempSlot = new ItemSlot();
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯을 가져오는 함수
    /// </summary>
    /// <param name="index">가져올 슬롯의 인덱스</param>
    /// <returns>index번째에 슬롯이 있으면 해당 슬롯을 리턴</returns>
    public ItemSlot GetSlot(uint index)
    {
        ItemSlot slot = null;
        if (IsValidSlotIndex(index))
        {
            slot = slots[index];
        }
        return slot;
    }

    /// <summary>
    /// 인벤토리에 아이템을 추가
    /// </summary>
    /// <param name="itemData">추가될 아이템의 종류</param>
    /// <returns>아이템 추가가 성공하면 true. 아니면 false </returns>
    public bool AddItem(ItemData itemData)
    {
        bool result = false;
        
        ItemSlot target = FindSameItem(itemData);   // 추가될 아이템과 같은 종류의 아이템이 있는지 확인

        if (target != null)
        {
            //같은 종류의 아이템이 있다.
            target.IncreaseSlotItem();  // 추가될 아이템과 같은 종류의 아이템이 있으면 갯수만 증가
            result = true;
        }
        else
        {
            //같은 종류의 아이템이 없다.
            ItemSlot empty = FindEmptySlot();       // 빈슬롯큐에서 비어있는 슬롯 가져오기 시도
            if (empty != null)
            {
                // 비어있는 아이템 슬롯을 가져왔다.
                empty.AssignSlotItem(itemData); // 수정 요구
                result = true;
                //Debug.Log($"{itemData.name}을 인벤토리에 추가.");
            }
            else
            {
                // 비어있는 아이템 슬롯이 없다(인벤토리가 가득찼다)
                Debug.Log($"인벤토리가 가득 차있다. {itemData.name}을 인벤토리에 추가할 수 없다.");
            }
        }        

        return result;
    }

    /// <summary>
    /// 아이템 슬롯에서 아이템을 빼는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 뺄 슬롯 인덱스</param>
    /// <param name="decreaseCount">아이템을 뺄 개수</param>
    /// <returns>true면 제거 성공. false면 실패</returns>
    public bool RemoveItemFromSlot(uint slotIndex, uint decreaseCount = 0)
    {
        bool result = false;

        if (IsSlotSetted(slotIndex))   // 적절한 인덱스 번호이면서 아이템이 설정되어있다.
        {
            // 지울 수 있는 slot의 인덱스다. 삭제 처리 진행
            //Debug.Log($"{slots[slotIndex].SlotItemData.name}을 인벤토리에서 제거합니다.");
            slots[slotIndex].DecreaseSlotItem(decreaseCount); // 슬롯에서 아이템 제거
            result = true;
        }
        else
        {
            // 적절하지 못한 인덱스다. 
            Debug.Log($"{slotIndex}는 적절하지 못한 인덱스이거나 비어 있습니다.");
        }
        return result;
    }

    /// <summary>
    /// 아이템 슬롯 하나를 완전히 비우는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 제거할 슬롯 인덱스</param>
    /// <returns>true면 제거 성공. false면 실패</returns>
    public bool ClearSlot(uint slotIndex)
    {
        bool result = false;

        if( IsSlotSetted(slotIndex) )   // 적절한 인덱스 번호이면서 아이템이 설정되어있다.
        {
            // 지울 수 있는 slot의 인덱스다. 삭제 처리 진행
            //Debug.Log($"{slots[slotIndex].SlotItemData.name}을 인벤토리에서 완전히 제거합니다.");
            slots[slotIndex].ClearSlotItem(); // 슬롯에서 아이템 제거
            result = true;
        }
        else
        {
            // 적절하지 못한 인덱스다. 
            Debug.Log($"{slotIndex}는 적절하지 못한 인덱스이거나 비어 있습니다.");
        }
        return result;
    }

    /// <summary>
    /// 인벤토리의 모든 슬롯을 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        Debug.Log("인벤토리 클리어");
        for (uint i = 0; i < SlotCount; i++)
        {
            ClearSlot(i);
        }
    }

    /// <summary>
    /// 시작 아이템 슬롯에서 도착 아이템 슬롯으로 아이템을 이동 시키는 함수
    /// </summary>
    /// <param name="from">시작 슬롯. 반드시 아이템이 있어야 함.</param>
    /// <param name="to">도착 슬롯</param>
    public void MoveItem(uint from, uint to)
    {
        if (IsSlotSetted(from) && IsValidSlotIndex(to))
        {
            // from은 적합한 인덱스이고 아이템이 할당되어 있다. 그리고 to는 적합한 인덱스이다.
            // from과 to가 같은 종류의 아이템인지 확인
            if ( slots[from].SlotItemData == slots[to].SlotItemData )
            {
                // 같은 종류의 아이템이면 from에서 to로 채워 넣기. 넘치면 
                uint overCount = slots[to].IncreaseSlotItem(slots[from].ItemCount);
                slots[from].DecreaseSlotItem(slots[from].ItemCount - overCount);
            }
            else
            {
                // 다른 종류의 아이템이면 갈아끼기
                //Debug.Log($"{from}에 있는 {slots[from].SlotItem.name} 아이템을 {to}로 이동합니다.");
                ItemData temp = slots[from].SlotItemData;               // from과 to의 아이템을 스왑
                uint tempCount = slots[from].ItemCount;
                slots[from].AssignSlotItem(slots[to].SlotItemData, slots[to].ItemCount);
                slots[to].AssignSlotItem(temp, tempCount);
            }            
        }
        else
        {
            // from은 적합하지 못한 인덱스이거나 아이템이 없다. 또는 to가 적합한 인덱스가 아니다.
            Debug.Log($"{from}에서 {to}로 아이템을 옮길 수 없습니다.");
        }
    }
    
    /// <summary>
    /// 아이템을 나누기 위해 시작 슬롯에서 아이템을 덜어내 임시 슬롯에 저장
    /// </summary>
    /// <param name="from">시작 슬롯</param>
    /// <param name="splitCount">나눌 수</param>
    public void SpliteItem_Start(uint from, uint splitCount)
    {
        if (IsSlotSetted(from) && splitCount < slots[from].ItemCount)   //.시작 슬롯 확인하고 아이템 개수 확인
        {
            spliteTempSlot.AssignSlotItem(slots[from].SlotItemData, splitCount);
            slots[from].DecreaseSlotItem(splitCount);
        }
    }

    /// <summary>
    /// 임시 슬롯에 저장되어있는 나눈 아이템을 도착 슬롯에 저장
    /// </summary>
    /// <param name="to">도착 슬롯</param>
    /// <param name="splitCount">나누어진 수</param>
    public void SpliteItem_End(uint to, uint splitCount)
    {
        if( spliteTempSlot.SlotItemData != null && IsValidSlotIndex(to))    // 임시 슬롯에 데이터가 있는지 확인하고 도착 슬롯 확인
        {
            if (slots[to].SlotItemData == null)
            {
                // to 슬롯이 비었으면 새롭게 할당
                slots[to].AssignSlotItem(spliteTempSlot.SlotItemData, splitCount);
                spliteTempSlot.ClearSlotItem();
            }
            else if (spliteTempSlot.SlotItemData == slots[to].SlotItemData)
            {
                // to 슬롯이 분할하려는 아이템과 같은 종류라면 그냥 추가
                uint overCount = slots[to].IncreaseSlotItem(splitCount);
                spliteTempSlot.DecreaseSlotItem(splitCount - overCount);
            }
        }
    }

    /// <summary>
    /// index변수값이 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스 값. false면 사용할 수 없는 인덱스 값</returns>
    bool IsValidSlotIndex(uint index)
    {
        // 적절하지 못한 인덱스 -> 인덱스 범위를 초과했을 때        
        return (index < SlotCount);
    }

    /// <summary>
    /// index변수값이 적절하고 해당 슬롯에 아이템이 들어있는지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true일 경우 인덱스가 적절하고 해당슬롯에 아이템이 들어있다.</returns>
    bool IsSlotSetted(uint index)
    {
        // 적절한 인덱스이고 슬롯에 아이템이 들어있다.
        return ((index < SlotCount) && (slots[index].SlotItemData != null));
    }

    /// <summary>
    /// 비어있는 아이템 슬롯을 돌려주는 함수
    /// </summary>
    /// <returns>null이면 비어있는 슬롯이 없음. 아닌 경우 비어있는 슬롯. </returns>
    ItemSlot FindEmptySlot()
    {
        ItemSlot slot = null;

        for(int i=0; i<SlotCount; i++)
        {
            if( slots[i].SlotItemData == null ) // 비어있는 슬롯 찾기
            {
                slot = slots[i];
                break;
            }
        }

        return slot;    // null 또는 빈 슬롯을 리턴
    }

    /// <summary>
    /// 같은 종류의 아이템이 있는 아이템슬롯을 돌려주는 함수
    /// </summary>
    /// <param name="itemData">같은지 비교할 아이템 데이터</param>
    /// <returns>파라메터와 같은 아이템이 들어있는 슬롯을 찾으면 해당 슬롯의 참조 리턴</returns>
    ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot slot = null;

        for (int i = 0; i < SlotCount; i++)
        {
            // 같은 아이템이 있는 슬롯 찾기
            // 그 슬롯에 빈 공간이 있는지도 확인
            if ((slots[i].SlotItemData == itemData) && (slots[i].ItemCount < slots[i].SlotItemData.maxStackCount)) 
            {                
                slot = slots[i];
                break;
            }
        }

        return slot;    // null 또는 같은 아이템이 있는 슬롯을 리턴
    }

    /// <summary>
    /// 인벤토리의 내용을 콘솔창에 출력해주는 함수
    /// </summary>
    public void Test_PrintInventory()
    {
        Debug.Log("Inven---------------------- ");
        for (int i = 0; i < SlotCount; i++)
        {
            Debug.Log($"{i} : {slots[i].SlotItemData?.name}, {slots[i].ItemCount}");  // 슬롯의 인덱스와 아이템의 이름 출력(없으면 안함)
        }
        Debug.Log("---------------------------- ");
    }
}
