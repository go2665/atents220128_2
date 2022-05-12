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

    Queue<ItemSlot> emptySlotQueue = null;

    public Inventory(int size = DEFAULT_INVENTORY_SIZE) //생성자
    {        
        slots = new ItemSlot[size];     // size에 지정된 숫자만큼 슬롯을 만든다.
        emptySlotQueue = new Queue<ItemSlot>(size);
        for (int i=0; i<size; i++)
        {
            slots[i] = new ItemSlot();
            emptySlotQueue.Enqueue(slots[i]);
        }
    }

    public bool AddItem(ItemData itemData)
    {
        bool result = false;
        ItemSlot empty = FindEmptySlot();
        if( empty != null )
        {
            // 비어있는 아이템 슬롯을 가져왔다.
            empty.slotItem = itemData;      // 수정 필수
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

    ItemSlot FindEmptySlot()
    {
        ItemSlot slot = null;
        if (emptySlotQueue.Count > 0)   //큐에 남은 슬롯이 있을 때
        {
            slot = emptySlotQueue.Dequeue();    //빈 슬롯을 하나 할당
        }
        return slot;
    }
}
