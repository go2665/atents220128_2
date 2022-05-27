using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// 지금 분리중인 아이템을 표시하기 위한 클래스
/// </summary>
public class SplittedItem : MonoBehaviour
{
    PointerEventData eventData = null;

    Image itemIcon = null;      // 아이콘 표시용 이미지
    Text itemCountText = null;  // 분리하는 아이템 개수 표시용 텍스트
    ItemSlot itemSlot = null;   // 분리작업을 시작한 슬롯
    public ItemSlot ItemSlot { get => itemSlot; }

    private int itemCount = 0;
    public int ItemCount 
    {
        get => itemCount;
        set
        {
            itemCount = value;
            itemCountText.text = itemCount.ToString();
        }
    }

    private void Awake()
    {
        itemIcon = GetComponentInChildren<Image>(); // 이미지 컴포넌트 찾기
        itemCountText = GetComponentInChildren<Text>(); //텍스트 컴포넌트 찾기
        GameManager.Inst.InventoryUI.InputActions.UI.Click.performed += OnClick;
    }

    private void OnEnable()
    {
        if(itemSlot != null)    // start전에도 한번 호출이 일어나기 때문에 
        {
            itemIcon.sprite = itemSlot.SlotItem.itemImage;  // itemSlot에 데이터가 있으면 슬롯에 있는 스프라이트를 사용하여 표시
        }
        GameManager.Inst.InventoryUI.InputActions.UI.Click.Enable();
    }

    private void OnDisable()
    {
        GameManager.Inst.InventoryUI.InputActions.UI.Click.Disable();
    }

    private void Start()
    {
        eventData = new PointerEventData(EventSystem.current);

        this.gameObject.SetActive(false);   // 시작할 때 닫기        
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();        
        eventData.position = mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results); // UI 레이케스트 사용하여 충돌되는 UI가 있는지 확인

        if (results.Count <= 0) // results.Count가 1개 이상이면 UI가 클릭된 것   
        {
            // UI를 클릭하지 않은 경우. 바닥에 버리기
            float dropRange = 2.0f;

            for (int i = 0; i < itemCount; i++)
            {
                GameObject obj = ItemFactory.GetItem(itemSlot.SlotItem.id);                 // 아이템 슬롯에 들어있는 아이템 데이터를 이용해 아이템 생성
                obj.transform.position = GameManager.Inst.MainPlayer.transform.position;    // 위치를 플레이어 위치로 변경

                Ray ray = Camera.main.ScreenPointToRay(mousePosition); // 마우스 포인터의 스크린 좌표를 이용해 레이를 구한다.
                RaycastHit[] hits = null;
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

                Vector3 randNoise = Random.insideUnitSphere * dropRange * 0.25f;
                obj.transform.Translate(randNoise.x, 0, randNoise.z);
            }
            Close();
        }
    }

    private void Update()
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 항상 마우스 위치를 따라다니기
    }

    /// <summary>
    /// 이 게임 오브젝트를 활성화 할 때(표시할 때) 사용하는 함수. (반드시 이 함수로만 진행되어야 함)
    /// </summary>
    /// <param name="targetSlot">아이템을 분리하는 작업을 진행하는 슬롯</param>
    /// <param name="count">분리하는 개수</param>
    public void Open(ItemSlot targetSlot, int count)
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 미리 마우스 위치로 옮겨놓기
        itemSlot = targetSlot;  // 대상 슬롯 저장
        ItemCount = count;      // 개수 저장
        this.gameObject.SetActive(true);    // 게임 오브젝트 활성화
    }

    public void Close()
    {
        itemSlot = null;                        // 널로 초기화
        this.gameObject.SetActive(false);       // 게임 오브젝트 비활성화
    }
}
