using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리를 화면상에 표시해주는 클래스
/// </summary>
public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject slotPrefab = null;    // 인벤토리 칸(Slot)의 프리팹

    private ItemSlotUI[] slotUIs = null;    // 생성된 인벤토리 칸(Slot)들
    private Inventory inven = null;         // 이 클래스가 표시할 인벤토리

    private int dragStartIndex = NOT_DRAG_START;        // 드래그 시작한 슬롯의 인덱스
    private const int NOT_DRAG_START = -1;
    private Image dragImage = null;

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
            slotUIs[i].ID = i;
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

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"드래그 중 : {eventData.position}");        
        if( dragImage != null )
        {
            dragImage.transform.position = eventData.position;
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : {eventData.pointerCurrentRaycast.gameObject.name}");

        GameObject startObj = eventData.pointerCurrentRaycast.gameObject;   // 드래그를 시작했을 때 마우스 포인터 위치에 있는 오브젝트 가져오기
        ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();    // ItemSlotUI를 가지는 아이템 슬롯인지 확인 및 데이터 가져오기
        if(slotUI != null)
        {
            dragStartIndex = slotUI.ID;     // ItemSlotUI가 있으면 드래그 시작 슬롯으로 인덱스 설정
            dragImage = slotUI.ItemImage;
            dragImage.transform.SetParent(this.transform.parent);  // 뒷쪽 슬롯에 아이콘이 가려지는 현상을 해결하기 위해 임시로 인벤토리를 부모 삼음
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {       
        GameObject endObj = eventData.pointerCurrentRaycast.gameObject; // 드래그를 시작했을 때 마우스 포인터 위치에 있는 오브젝트 가져오기
        if (endObj != null) // UI위에서 드래그를 끝냈는지 확인
        {
            Debug.Log($"드래그 종료 : {eventData.pointerCurrentRaycast.gameObject.name}");
            ItemSlotUI slotUI = endObj.GetComponent<ItemSlotUI>();  // ItemSlotUI를 가지는 아이템 슬롯인지 확인 및 데이터 가져오기
            if (slotUI != null && dragStartIndex != NOT_DRAG_START) // ItemSlotUI가 있고 드래그를 시작한 상황이면
            {
                // 의도되로 사용된 케이스
                inven.MoveItem((uint)dragStartIndex, (uint)slotUI.ID);  // 두 슬롯의 아이템 서로 변경
            }
        }

        // 다 끝났으니 원상복구
        if (dragImage != null && dragStartIndex != NOT_DRAG_START)
        {
            dragImage.transform.SetParent(slotUIs[dragStartIndex].transform);   // 부모를 원래 슬롯으로 조정하기
            dragImage.transform.localPosition = Vector3.zero;                   // 로컬 포지션으로 (0,0)으로 설정(원래 위치)
            dragStartIndex = NOT_DRAG_START;                                    // 드래그 끝났다고 표시
            dragImage = null;                                                   // 움직일 이미지도 null로 표시
        }
    }
}
