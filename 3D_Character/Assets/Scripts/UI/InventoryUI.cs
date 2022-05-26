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
    public float dropRange = 2.0f;

    private DetailInfoUI detail = null;
    public DetailInfoUI Detail { get => detail; }
    private ItemSpliter spliter = null;
    public ItemSpliter Spliter { get => spliter; }
    private SplittedItem splittedItem = null;
    public SplittedItem SplittedItem { get => splittedItem; }

    private ItemSlotUI[] slotUIs = null;    // 생성된 인벤토리 칸(Slot)들
    private Inventory inven = null;         // 이 클래스가 표시할 인벤토리

    private int dragStartIndex = NOT_DRAG_START;        // 드래그 시작한 슬롯의 인덱스
    private const int NOT_DRAG_START = -1;
    private Image dragImage = null;
    private Transform slotParent = null;

    public delegate void InventoryDelegate();
    public InventoryDelegate onInventoryOpen = null;    // 인벤토리가 열릴 때 실행될 델리게이트
    public InventoryDelegate onInventoryClose = null;   // 인벤토리가 닫힐 때 실행될 델리게이트

    void Awake()
    {
        slotParent = transform.Find("SlotParent");
        detail = GetComponentInChildren<DetailInfoUI>();
        spliter = GetComponentInChildren<ItemSpliter>();
        splittedItem = GetComponentInChildren<SplittedItem>();

        Button closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
    }

    private void Start()
    {
        InitializeInventory(GameManager.Inst.MainPlayer.Inven);     //플레이어가 가지고 있는 인벤토리를 표시하도록 설정
        GameManager.Inst.MainPlayer.onInventoryOnOff = InventoryOnOffSwitch;
        this.gameObject.SetActive(false);
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
            GameObject obj = Instantiate(slotPrefab, slotParent);       // slot을 생성
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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log($"드래그 시작 : {eventData.pointerCurrentRaycast.gameObject.name}");

            GameObject startObj = eventData.pointerCurrentRaycast.gameObject;   // 드래그를 시작했을 때 마우스 포인터 위치에 있는 UI 오브젝트 가져오기
            ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();    // ItemSlotUI를 가지는 아이템 슬롯인지 확인 및 데이터 가져오기
            if (slotUI != null)
            {
                dragStartIndex = slotUI.ID;     // ItemSlotUI가 있으면 드래그 시작 슬롯으로 인덱스 설정
                dragImage = slotUI.ItemImage;
                dragImage.transform.SetParent(this.transform.parent);  // 뒷쪽 슬롯에 아이콘이 가려지는 현상을 해결하기 위해 임시로 인벤토리를 부모 삼음
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject endObj = eventData.pointerCurrentRaycast.gameObject; // 드래그가 끝났을 때 마우스 포인터 위치에 있는 UI 오브젝트 가져오기

            if (endObj != null)
            {
                // UI위에서 드래그를 끝냄
                //Debug.Log($"드래그 종료 : {eventData.pointerCurrentRaycast.gameObject.name}");
                ItemSlotUI endSlotUI = endObj.GetComponent<ItemSlotUI>();  // ItemSlotUI를 가지는 아이템 슬롯인지 확인 및 데이터 가져오기
                if (endSlotUI != null && dragStartIndex != NOT_DRAG_START) // ItemSlotUI가 있고 드래그를 시작한 상황이면
                {
                    // 드래그가 성공적으로 완료된 경우

                    ItemSlot startSlot = inven.GetSlot((uint)dragStartIndex);   // 드래그를 시작한 슬롯
                    if( startSlot.SlotItem == endSlotUI.ItemSlot.SlotItem ) // 시작슬롯과 도착슬롯의 아이템이 같은 경우
                    {
                        // 시작슬롯과 도착슬롯의 아이템을 합치면 최대치를 넘어서는지 확인
                        if ( startSlot.ItemCount + endSlotUI.ItemSlot.ItemCount > endSlotUI.ItemSlot.SlotItem.maxStackCount )
                        {
                            // 시작슬롯과 도착슬롯의 아이템을 합치면 슬롯의 최대 스택 사이즈를 넘어서는경우
                            // 도착슬롯의 아이템 개수가 최대치가 될때까지만 옮기기
                            if( endSlotUI.ItemSlot.ItemCount < endSlotUI.ItemSlot.SlotItem.maxStackCount )
                            {
                                // 도착슬롯의 아이템 개수가 아직 추가될 수 있는 상황일 때

                                // 도착지점의 아이템 개수가 최대치가 되는 값 구하기
                                int toMax = endSlotUI.ItemSlot.SlotItem.maxStackCount - endSlotUI.ItemSlot.ItemCount;   
                                endSlotUI.ItemSlot.IncreaseSlotItem(toMax); // 도착 슬롯은 최대치까지 올리고
                                startSlot.DecreaseSlotItem(toMax);          // 시작 슬롯은 추가한만큼 제거
                            }
                        }
                        else
                        {
                            // 전부 옮기기
                            endSlotUI.ItemSlot.IncreaseSlotItem(startSlot.ItemCount);
                            startSlot.ReleaseSlotItem();
                        }                        
                    }
                    else
                    {
                        inven.MoveItem((uint)dragStartIndex, (uint)endSlotUI.ID);  // 두 슬롯의 아이템 서로 변경
                    }
                    detail.Open(endSlotUI.ItemSlot.SlotItem);
                }
            }
            else
            {
                // UI 바깥쪽에서 드래그를 끝냄
                ItemSlot itemSlot = inven.GetSlot((uint)dragStartIndex);            // 드래그 시작 위치에 있는 아이템 슬롯 가져오기
                if (itemSlot != null && itemSlot.SlotItem != null)                  // 아이템 슬롯에서 시작했고 슬롯에 아이템이 있는 경우만 처리
                {
                    //itemSlot.ItemCount;
                    for (int i = 0; i < itemSlot.ItemCount; i++)
                    {
                        GameObject obj = ItemFactory.GetItem(itemSlot.SlotItem.id);                 // 아이템 슬롯에 들어있는 아이템 데이터를 이용해 아이템 생성
                        obj.transform.position = GameManager.Inst.MainPlayer.transform.position;    // 위치를 플레이어 위치로 변경

                        Ray ray = Camera.main.ScreenPointToRay(eventData.position); // 마우스 포인터의 스크린 좌표를 이용해 레이를 구한다.
                        RaycastHit[] hits = null;
                        //LayerMask.NameToLayer("Ground")   // 결과는 레이어 번호. 리턴값은 7
                        //LayerMask.GetMask("Ground")       // 결과는 비트 마스크. 리턴값은 2진수로 0b_10000000 = 십진수로 128
                        //Debug.Log($"NameToLayer : {LayerMask.NameToLayer("Ground")}");
                        //Debug.Log($"GetMask : {LayerMask.GetMask("Ground")}");
                        hits = Physics.RaycastAll(ray, 1000.0f, LayerMask.GetMask("Ground"));  // Ground 레이어로 설정된 오브젝트와 레이를 충돌검사한다.
                        if (hits.Length > 0)
                        {
                            // 최소 하나 이상 피킹 되었을 때
                            Vector3 playerToDrop = hits[0].point - GameManager.Inst.MainPlayer.transform.position;  // 플레이어 위치에서 드랍지점으로 가는 방향 백터 구하기
                            if (dropRange * dropRange < playerToDrop.sqrMagnitude) // 방향백터의 길이를 이용해서 dropRange 안인지 밖인지 확인
                            {
                                // dropRange 바깥에 아이템을 드랍했다.

                                // 방향백터를 단위백터로 만들고 dropRange를 곱해서 dropRange를 반지름으로 가지는 원의 표면에 아이템을 배치시킨다.
                                obj.transform.Translate(playerToDrop.normalized * dropRange);
                            }
                            else
                            {
                                // dropRange 안에 아이템을 드랍했다.

                                // 그냥 드랍한 위치에 아이템을 배치한다.
                                obj.transform.position = hits[0].point;
                            }
                        }
                        else
                        {
                            // Ground가 하나도 피킹되지 않았을 때
                            Vector3 randDrop = Random.insideUnitSphere * dropRange;     // 반지름이 dropRange인 구안의 랜덤한 위치 구하기
                            obj.transform.Translate(randDrop.x, 0, randDrop.z);         // 높이를 제외하고 랜덤한 위치를 적용하기
                        }

                        Vector3 randNoise = Random.insideUnitSphere * dropRange;
                        obj.transform.Translate(randNoise.x, 0, randNoise.z);       // 정확한 지점말고 약간 노이즈를 주는 느낌
                    }

                    inven.RemoveItem((uint)dragStartIndex);     // 인벤토리에서 아이템 제거
                }
            }

            // 다 끝났으니 원상복구
            RollbackDragImage();
        }// 움직일 이미지도 null로 표시        
    }

    private void RollbackDragImage()
    {
        if (dragImage != null && dragStartIndex != NOT_DRAG_START)
        {
            dragImage.transform.SetParent(slotUIs[dragStartIndex].transform);   // 부모를 원래 슬롯으로 조정하기
            dragImage.transform.localPosition = Vector3.zero;                   // 로컬 포지션으로 (0,0)으로 설정(원래 위치)
            dragImage.transform.SetAsFirstSibling();                            // drag 이미지를 첫번째 자식으로 변경
            dragStartIndex = NOT_DRAG_START;                                    // 드래그 끝났다고 표시
            dragImage = null;
        }
    }

    void Close()
    {
        RollbackDragImage();
        onInventoryClose?.Invoke();         // 닫힐 때 델리게이트 실행
        this.gameObject.SetActive(false);
    }

    void Open()
    {
        this.gameObject.SetActive(true);
        onInventoryOpen?.Invoke();          // 열릴 때 델리게이트 실행
    }

    void InventoryOnOffSwitch()
    {
        if( this.gameObject.activeSelf )
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
