using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리를 화면상에 표시해주는 클래스
/// </summary>
public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab = null;    // 인벤토리 칸(Slot)의 프리팹

    private ItemSlotUI[] slotUIs = null;    // 생성된 인벤토리 칸(Slot)들
    private Inventory inven = null;         // 이 클래스가 표시할 인벤토리

    private void Start()
    {
        InitializeInventory(GameManager.Inst.MainPlayer.Inven);     //플레이어가 가지고 있는 인벤토리를 표시하도록 설정
    }

    /// <summary>
    /// 파라메터로 받은 인벤토리를 표시하기 위해 초기화
    /// </summary>
    /// <param name="newInven">표시할 인벤토리</param>
    public void InitializeInventory(Inventory newInven)
    {
        RemoveAllSlots();       // 기존에 존재하던 슬롯들을 모두 삭제
        inven = newInven;       // 새 인벤토리를 inven변수에 설정
        slotUIs = new ItemSlotUI[inven.SlotCount];  // inven의 크기에 맞춰 slotUI들이 들어갈 공간 확보
        for ( int i=0;i< inven.SlotCount; i++)      // 슬롯 갯수만큼 for 실행
        {
            GameObject obj = Instantiate(slotPrefab, this.transform);   // slot을 생성
            obj.name = $"{slotPrefab.name}_{i}";                        // slot 이름 변경
            slotUIs[i] = obj.GetComponent<ItemSlotUI>();                // ItemSlotUI 컴포넌트 찾아서 캐싱
            slotUIs[i].ItemSlot = inven.GetSlot((uint)i);               // slot과 slotUI를 연결
        }
        Refresh();  // 인벤토리 내부를 다시 그리기
    }

    /// <summary>
    /// 기존에 존재하던 자식 Slot들을 다 제거
    /// </summary>
    public void RemoveAllSlots()
    {
        ItemSlotUI[] slots = transform.GetComponentsInChildren<ItemSlotUI>();   // ItemSlotUI으로 찾아서
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);   // 모두 제거
        }
    }

    /// <summary>
    /// 모든 자식 슬롯의 이미지들 갱신
    /// </summary>
    private void Refresh()
    {
        foreach(var slot in slotUIs)
        {
            slot.Refresh();
        }
    }
}
