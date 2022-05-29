using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MovingSlotUI : MonoBehaviour
{
    private PointerEventData eventData = null;
    ItemSlot itemSlot = null;
    private ItemData itemData = null;
    private uint itemCount = 0;
    private int splitStartID = -1;
    private bool splitMode = false;

    private Image itemImage = null;
    private Text countText = null;

    public bool SplitMode { get => splitMode; }
    public uint SplitStartID { get => (uint)splitStartID; }
    public uint SplitCount { get => itemCount; }

    public void SetDragItemData(ItemData data, uint count)
    {
        itemData = data;
        itemCount = count;

        Refresh();
    }

    public void SetSplitItemData(ItemSlot slot, uint count, int startID)
    {
        splitMode = true;
        itemSlot = slot;
        itemData = slot.SlotItemData;
        itemCount = count;
        splitStartID = startID;

        Refresh();
    }

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        countText = GetComponentInChildren<Text>();
        GameManager.Inst.InventoryUI.UIActions.Click.performed += OnClick;
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
        eventData = new PointerEventData(EventSystem.current);
        GameManager.Inst.InventoryUI.onInventoryDragStart += Open;
        GameManager.Inst.InventoryUI.onInventoryDragEnd += Close;
        GameManager.Inst.InventoryUI.Spliter.onSpliterOK += Open;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 항상 마우스 위치를 따라다니기
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (this.splitMode)
        {
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
                ItemFactory.GetItems(itemData.id, position, itemCount);    // 드랍한 개수만큼 아이템 생성
                itemSlot.DecreaseSlotItem(itemCount);   // 드랍한 개수만큼 원래 슬롯에서 빼기
                Close();
            }
        }
    }

    public void Open()
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 미리 마우스 위치로 옮겨놓기
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        splitMode = false;
        this.gameObject.SetActive(false);
    }

    void Refresh()
    {
        itemImage.sprite = itemData.itemImage;
        countText.text = itemCount.ToString();
    }
}
