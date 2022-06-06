using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void InventoryDelegate();

/// <summary>
/// 인벤토리를 화면상에 표시해주는 클래스
/// </summary>
public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // 주요 데이터----------------------------------------------------------------------------------
    /// <summary>
    /// 인벤토리 칸(Slot)의 프리팹
    /// </summary>
    public GameObject slotPrefab = null;

    /// <summary>
    /// 아이템을 버릴 때 버려지는 반경
    /// </summary>
    public float dropRange = 2.0f;

    /// <summary>
    /// 이 클래스가 표시할 인벤토리
    /// </summary>
    private Inventory inven = null;

    /// <summary>
    /// 드래그 시작 인덱스 기록용
    /// </summary>
    private int dragStartIndex = NOT_DRAG_START;

    /// <summary>
    /// 인벤토리 용 액션맵 접근을 위한 클래스
    /// </summary>
    private PlayerInputActions inputActions = null;

    // 상수들 -------------------------------------------------------------------------------------
    /// <summary>
    /// 드래그가 시작되지 않았음을 표시하기 위한 상수
    /// </summary>
    private const int NOT_DRAG_START = -1;


    // 하부 UI들-----------------------------------------------------------------------------------
    private ItemSlotUI[] slotUIs = null;        // 생성된 인벤토리 칸(Slot)들
    private DetailInfoUI detail = null;         // 아이템 상세 정보창(접근의 편의를 위해 인벤토리UI가 가지고 있지만 직접 다루는 건 각 슬롯들에서만 하기)
    private ItemSpliterUI spliter = null;       // 아이템 나누기 창
    private MovingSlotUI movingSlot = null;     // 드래그 이동이나 아이템 나누기 할 때 보이는 임시 오브젝트
    private Transform slotParent = null;        // 슬롯UI들이 생성되며 붙을 부모 트랜스폼

    // 프로퍼티들-----------------------------------------------------------------------------------
    /// <summary>
    /// 디테일 창 접근용 프로퍼티
    /// </summary>
    public DetailInfoUI Detail { get => detail; }
    /// <summary>
    /// 아이템 나누기 창 접근용 프로퍼티
    /// </summary>
    public ItemSpliterUI Spliter { get => spliter; }
    /// <summary>
    /// 드래그 이동이나 아이템 나누기 할 때 보이는 임시 오브젝트 용 프로퍼티
    /// </summary>
    public MovingSlotUI MovingSlot { get => movingSlot; }
    /// <summary>
    /// 인벤토리용 UI 액션맵에 접근용 프로퍼티
    /// </summary>
    public PlayerInputActions.UIActions UIActions { get => inputActions.UI; }


    // 델리게이트들 --------------------------------------------------------------------------------    
    public InventoryDelegate onInventoryOpen = null;    // 인벤토리가 열릴 때 실행될 델리게이트
    public InventoryDelegate onInventoryClose = null;   // 인벤토리가 닫힐 때 실행될 델리게이트
    public InventoryDelegate onInventoryDragStart = null;   // 인벤토리 드래그가 시작될 때 실행될 델리게이트
    public InventoryDelegate onInventoryDragEnd = null;     // 인벤토리 드래그가 끝날 때 실행될 델리게이트
    public InventoryDelegate onInventoryDragOutEnd = null;  // 인벤토리 드래그가 슬롯UI 밖에서 끝났을 때 실행될 델리게이트
    public InventoryDelegate onInventorySplittingStart = null;   // 인벤토리 드래그가 시작될 때 실행될 델리게이트
    public InventoryDelegate onInventorySplittingEnd = null;     // 인벤토리 드래그가 끝날 때 실행될 델리게이트



    // 함수들 -------------------------------------------------------------------------------------
    /// <summary>
    /// 파라메터로 받은 인벤토리를 표시하기 위해 초기화
    /// </summary>
    /// <param name="newInven">표시할 인벤토리</param>
    public void InitializeInventory(Inventory newInven)
    {
        RemoveAllSlots();       // 기존에 존재하던 슬롯들을 모두 삭제
        inven = newInven;       // 새 인벤토리를 inven변수에 설정
        slotUIs = new ItemSlotUI[inven.SlotCount];  // inven의 크기에 맞춰 slotUI들이 들어갈 공간 확보
        for (int i = 0; i < inven.SlotCount; i++)      // 슬롯 갯수만큼 for 실행
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
    /// Inventory의 SpliteItem_Start 래핑
    /// </summary>
    /// <param name="from">시작 슬롯의 아이디</param>
    /// <param name="splitCount">분리할 개수</param>
    public void SpliteItem_Start(uint from, uint splitCount)
    {
        onInventorySplittingStart();                    // 디테일창 pause
        inven.SpliteItem_Start(from, splitCount);       // 인벤토리에서 실제 처리
        movingSlot.SetTargetSlot(inven.SpliteTempSlot); // 무빙 슬롯 표시 위해 슬롯 설정
    }

    /// <summary>
    /// Inventory의 SpliteItem_End 래핑
    /// </summary>
    /// <param name="to">도착 슬롯의 아이디</param>
    /// <param name="splitCount">분리된 개수</param>
    public void SpliteItem_End(uint to, uint splitCount)
    {
        inven.SpliteItem_End(to, splitCount);       // 인벤토리에서 실제 처리
        onInventorySplittingEnd();                  // 디테일창 restart
    }

    /// <summary>
    /// 모든 자식 슬롯UI들 갱신
    /// </summary>
    private void Refresh()
    {
        foreach (var slot in slotUIs)
        {
            slot.Refresh();
        }
    }    

    /// <summary>
    /// 인벤토리 열기
    /// </summary>
    private void Open()
    {
        this.gameObject.SetActive(true);
        onInventoryOpen?.Invoke();          // 열릴 때 델리게이트 실행
    }

    /// <summary>
    /// 인벤토리 닫기
    /// </summary>
    private void Close()
    {
        onInventoryClose?.Invoke();         // 닫힐 때 델리게이트 실행
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 토글용(열고 닫기). 풀레이어 인벤토리 단축키에 연결
    /// </summary>
    private void InventoryOnOffSwitch()
    {
        if (this.gameObject.activeSelf)
        {
            Close();
        }
        else
        {
            Open();
        }
    }


    // 유니티 이벤트 함수들 ------------------------------------------------------------------------
    void Awake()
    {
        // 캐싱용 오브젝트 찾기
        slotParent = transform.Find("SlotParent");
        // 캐싱용 컴포넌트 찾기
        detail = GetComponentInChildren<DetailInfoUI>();            // 디테일 창
        spliter = GetComponentInChildren<ItemSpliterUI>();          // 아이템 나누기 창
        movingSlot = GetComponentInChildren<MovingSlotUI>();        // 드래그나 아이템 나눌때 보일 임시 슬롯
        Button closeButton = transform.Find("CloseButton").GetComponent<Button>();  // 닫기 버튼
        closeButton.onClick.AddListener(Close);     // 클릭 이벤트 등록

        inputActions = new PlayerInputActions();
    }

    private void Start()
    {
        InitializeInventory(GameManager.Inst.MainPlayer.Inven);     // 플레이어가 가지고 있는 인벤토리를 표시하도록 설정
        GameManager.Inst.MainPlayer.onInventoryOnOff = InventoryOnOffSwitch;
        movingSlot.SetTargetSlot(inven.SpliteTempSlot);             // 기본 무빙 슬롯용 슬롯 설정(널 방지용)
        this.gameObject.SetActive(false);
    }


    // 드래그 용 마우스 입력 이벤트들 ---------------------------------------------------------------
    /// <summary>
    /// 하는 일 없음. 드래그 시작과 끝을 체크하기 위해선 IDragHandler가 반드시 필요하기 때문에 추가. 
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"드래그 중 : {eventData.position}");
    }

    /// <summary>
    /// 드래그 시작 처리
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log($"드래그 시작 : {eventData.pointerCurrentRaycast.gameObject.name}");            

            GameObject startObj = eventData.pointerCurrentRaycast.gameObject;   // 드래그를 시작했을 때 마우스 포인터 위치에 있는 UI 오브젝트 가져오기
            ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();            // ItemSlotUI를 가지는 아이템 슬롯인지 확인 및 데이터 가져오기
            if (slotUI != null)
            {
                dragStartIndex = slotUI.ID;                                     // ItemSlotUI가 있으면 드래그 시작 슬롯으로 인덱스 설정                
                movingSlot.SetTargetSlot(slotUI.ItemSlot);                      // movingSlot 데이터 설정
                onInventoryDragStart();
            }
        }
    }

    /// <summary>
    /// 드래그 종료 처리
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject endObj = eventData.pointerCurrentRaycast.gameObject;     // 드래그가 끝났을 때 마우스 포인터 위치에 있는 UI 오브젝트 가져오기

            if (endObj != null)
            {
                // 슬롯UI 위에서 드래그를 끝냄
                // Debug.Log($"드래그 종료 : {eventData.pointerCurrentRaycast.gameObject.name}");
                ItemSlotUI endSlotUI = endObj.GetComponent<ItemSlotUI>();       // ItemSlotUI를 가지는 아이템 슬롯인지 확인 및 데이터 가져오기
                if (endSlotUI != null && dragStartIndex != NOT_DRAG_START)      // ItemSlotUI가 있고 드래그를 시작한 상황이면
                {
                    // 드래그가 성공적으로 완료된 경우
                    inven.MoveItem((uint)dragStartIndex, (uint)endSlotUI.ID);   // 아이템 이동(넘치거나 하는 것들은 inven이 알아서 처리)
                }
            }
            onInventoryDragOutEnd();    // 일단은 디테일창 닫기용
            onInventoryDragEnd();       // 인벤토리 드래그 종료
        }
    }
}
