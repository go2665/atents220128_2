using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MovingSlotUI : MonoBehaviour
{
    // 주요 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// MovingSlot이 표현할 슬롯
    /// </summary>
    private ItemSlot itemSlot = null;
    /// <summary>
    /// UI 클릭 체크를 확인하기 위한 이벤트 데이터
    /// </summary>
    private PointerEventData eventData = null;

    // UI 캐싱용 변수 ------------------------------------------------------------------------------
    private Image itemImage = null; // 슬롯의 아이템 이미지
    private Text countText = null;  // 슬롯의 아이템 개수

    // 프로퍼티들 ----------------------------------------------------------------------------------
    /// <summary>
    /// 나누는 아이템 개수. 읽기 전용
    /// </summary>
    public uint SplitCount { get => itemSlot.ItemCount; }
    /// <summary>
    /// MovingSlot이 표현할 슬롯. 읽기 전용
    /// </summary>
    public ItemSlot ItemSlot { get => itemSlot; }

    // 함수들 -------------------------------------------------------------------------------------
    /// <summary>
    /// 표현할 대상 슬롯 지정.
    /// </summary>
    /// <param name="slot">표현할 슬롯</param>
    public void SetTargetSlot(ItemSlot slot)
    {
        itemSlot = slot;    // 슬롯 설정하고
        Refresh();          // 화면 갱신
    }

    /// <summary>
    /// MovingSlot 열기
    /// </summary>
    public void Open()
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 미리 마우스 위치로 옮겨놓기
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// MovingSlot 닫기
    /// </summary>
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// MovingSlot 이미지와 텍스트 갱신
    /// </summary>
    void Refresh()
    {
        if (itemSlot.SlotItemData != null)
        {
            itemImage.sprite = itemSlot.SlotItemData.itemImage;
            countText.text = itemSlot.ItemCount.ToString();
        }
    }


    // 유니티 이벤트 함수들 ------------------------------------------------------------------------
    private void Awake()
    {
        itemImage = GetComponent<Image>();
        countText = GetComponentInChildren<Text>();
        GameManager.Inst.InventoryUI.UIActions.Click.performed += OnClick;  // 클릭용 함수 등록
    }

    private void OnEnable()
    {
        GameManager.Inst.InventoryUI.UIActions.Click.Enable();
    }

    private void OnDisable()
    {
        GameManager.Inst.InventoryUI.UIActions.Click.Disable();
    }

    private void Start()
    {
        eventData = new PointerEventData(EventSystem.current);      // eventData 캐싱
        GameManager.Inst.InventoryUI.onInventoryDragStart += Open;  // 인벤토리에서 드래그 시작하면 열기
        GameManager.Inst.InventoryUI.onInventoryDragEnd += Close;   // 인벤토리에서 드래그 끝내면 닫기
        GameManager.Inst.InventoryUI.Spliter.onSpliterOK += Open;   // 아이템 나누기 창에서 OK누르면 열기
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 항상 마우스 위치를 따라다니기
    }

    /// <summary>
    /// UI 바깥에 아이템을 버리는 용도. 마우스 클릭 때 실행(드래그를 끝낼 때도 실행됨)
    /// </summary>
    /// <param name="context"></param>
    private void OnClick(InputAction.CallbackContext context)
    {
        //바깥에 떨어지는 것 처리        
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        eventData.position = mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results); // UI 레이케스트 사용하여 충돌되는 UI가 있는지 확인

        if (results.Count <= 0) // results.Count가 1개 이상이면 UI가 클릭된 것(슬롯을 클릭한 경우는 슬롯이 처리)
        {
            // UI를 클릭하지 않은 경우. 바닥에 버리기
            float dropRange = 2.0f;

            // 아이템 드랍할 위치 계산하기
            Vector3 position = new Vector3();
            Ray ray = Camera.main.ScreenPointToRay(eventData.position); // 마우스 포인터의 스크린 좌표를 이용해 레이를 구한다.
            RaycastHit[] hits = null;
            hits = Physics.RaycastAll(ray, 1000.0f, LayerMask.GetMask("Ground"));  // Ground 레이어로 설정된 오브젝트와 레이를 충돌검사한다.
            if (hits.Length > 0)
            {
                Vector3 playerToDrop = hits[hits.Length - 1].point - GameManager.Inst.MainPlayer.transform.position;  // 플레이어 위치에서 드랍지점으로 가는 방향 백터 구하기
                if (dropRange * dropRange < playerToDrop.sqrMagnitude) // 방향백터의 길이를 이용해서 dropRange 안인지 밖인지 확인
                {
                    // dropRange 바깥에 아이템을 드랍했다.

                    // 방향백터를 단위백터로 만들고 dropRange를 곱해서 dropRange를 반지름으로 가지는 원의 표면에 아이템을 배치시킨다.
                    position = playerToDrop.normalized * dropRange;
                }
                else
                {
                    // dropRange 안에 아이템을 드랍했다.

                    // 그냥 드랍한 위치에 아이템을 배치한다.
                    position = hits[hits.Length - 1].point;
                }
            }
            ItemFactory.GetItems(itemSlot.SlotItemData.id, position, itemSlot.ItemCount);    // 드랍한 개수만큼 아이템 생성
            itemSlot.DecreaseSlotItem(itemSlot.ItemCount);   // 드랍한 개수만큼 원래 슬롯에서 빼기
            Close();
        }        
    }
}
