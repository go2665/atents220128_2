using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //인벤토리의 기능
    // - 아이템을 가져야 한다. => 아이템 추가, 삭제, 아이템 칸(ItemSlot)이 있어야 한다.
    // - 인벤토리에 빈칸이 있는지 확인할 수 있어야 함
    // - 아이템 위치 변경

    // - 같은 종류의 아이템 쌓기(stacking)
    // - 아이템 사용하기
    // - 아이템 버리기


    // ItemSlot들을 담을 자료구조가 요구하는 조건
    // 1. 갯수 변화가 없다.
    // 2. 각 항목에 빠르게 접근할 수 있어야 한다.
    //  => 배열이 가장 적합하다.

    ItemSlot[] slots = null;                // 아이템 칸 역할을 할 클래스
    const int DEFAULT_INVENTORY_SIZE = 5;   // 기본 아이템 칸 수

    Queue<ItemSlot> emptySlotQueue = null;  // 비어있는 슬롯을 관리하는 큐

    /// <summary>
    /// 인벤토리에 들어갈 슬롯과 빈슬롯 큐 초기화
    /// </summary>
    /// <param name="size">인벤토리의 칸수. 기본값 5. </param>
    public Inventory(int size = DEFAULT_INVENTORY_SIZE) //생성자
    {        
        slots = new ItemSlot[size];                     // size에 지정된 숫자만큼 슬롯을 만든다.
        emptySlotQueue = new Queue<ItemSlot>(size);     // 빈 슬롯을 관리하는 큐의 기본 크기 설정
        for (int i=0; i<size; i++)
        {
            slots[i] = new ItemSlot();                  // 슬롯 생성
            emptySlotQueue.Enqueue(slots[i]);           // 생성한 슬롯은 우선 빈슬롯큐에 추가
        }
    }

    /// <summary>
    /// 인벤토리에 아이템을 추가
    /// </summary>
    /// <param name="itemData">추가될 아이템의 종류</param>
    /// <returns>아이템 추가가 성공하면 true. 아니면 false </returns>
    public bool AddItem(ItemData itemData)
    {
        bool result = false;
        ItemSlot empty = FindEmptySlot();       // 빈슬롯큐에서 비어있는 슬롯 가져오기 시도
        if( empty != null )
        {
            // 비어있는 아이템 슬롯을 가져왔다.
            empty.AssignSlotItem(itemData); // 수정 요구
            result = true;
            Debug.Log($"{itemData.name}을 인벤토리에 추가.");
        }
        else
        {
            // 비어있는 아이템 슬롯이 없다(인벤토리가 가득찼다)
            Debug.Log($"인벤토리가 가득 차있다. {itemData.name}을 인벤토리에 추가할 수 없다.");
        }

        return result;
    }

    /// <summary>
    /// 인벤토리에서 아이템을 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 제거할 슬롯 인덱스</param>
    /// <returns>true면 제거 성공. false면 실패</returns>
    public bool RemoveItem(uint slotIndex)
    {
        bool result = false;

        if( IsValidSlotIndex(slotIndex) )   // 적절한 인덱스 번호인지 확인
        {
            // 지울 수 있는 slot의 인덱스다. 삭제 처리 진행
            Debug.Log($"{slots[slotIndex].SlotItem.name}을 인벤토리에서 제거합니다.");
            slots[slotIndex].ReleaseSlotItem(); // 슬롯에서 아이템 제거
            result = true;
        }
        else
        {
            // 적절하지 못한 인덱스다. 
            Debug.Log($"{slotIndex}는 적절하지 못한 인덱스입니다.");
        }        

        return result;
    }

    /// <summary>
    /// index변수값이 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스 값. false면 사용할 수 없는 인덱스 값</returns>
    bool IsValidSlotIndex(uint index)
    {
        // 적절하지 못한 인덱스
        //  - 인덱스 범위를 초과했을 때
        //  - 해당 슬롯의 itemData가 null인 경우
        return ((index < slots.Length) && (slots[index].SlotItem != null));
    }

    /// <summary>
    /// 비어있는 아이템 칸을 돌려주는 함수
    /// </summary>
    /// <returns>null이면 비어있는 슬롯이 없음. 아닌 경우 비어있는 슬롯. </returns>
    ItemSlot FindEmptySlot()
    {
        ItemSlot slot = null;
        if (emptySlotQueue.Count > 0)           //큐에 남은 슬롯이 있을 때
        {
            slot = emptySlotQueue.Dequeue();    //빈 슬롯을 하나 할당
        }
        return slot;    // null 또는 빈 슬롯을 리턴
    }
}
