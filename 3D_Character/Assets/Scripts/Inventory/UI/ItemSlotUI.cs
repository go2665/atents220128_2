using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// ItemSlot을 표시해주는 클래스
/// </summary>
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    // 주요 데이터----------------------------------------------------------------------------------
    /// <summary>
    /// 슬롯의 식별용 번호. 인벤토리UI의 몇번째 슬롯인지 표시. 0번부터 시작
    /// </summary>
    private int id = 0;
    /// <summary>
    /// 이 슬롯이 붙어있는 인벤토리UI
    /// </summary>
    private InventoryUI invenUI = null;
    /// <summary>
    /// 이 클래스가 표시할 ItemSlot
    /// </summary>
    private ItemSlot itemSlot = null;



    // 캐싱용 컴포넌트들----------------------------------------------------------------------------    
    /// <summary>
    /// 아이템의 이미지를 표시할 UI Image
    /// </summary>
    private Image itemImage = null;
    /// <summary>
    /// 아이템의 개수를 표시할 텍스트
    /// </summary>
    private Text itemCountText = null;
    /// <summary>
    /// 아이템의 장비 여부를 표시하는 텍스트
    /// </summary>
    private Text itemEquipText = null;
    /// <summary>
    /// 디테일창의 RectTransform. 계산에 자주 사용되어 캐싱.
    /// </summary>
    private RectTransform detailRect = null;



    // 프로퍼티들-----------------------------------------------------------------------------------
    /// <summary>
    /// 이 UI와 연결된 ItemSlot
    /// </summary>
    public ItemSlot ItemSlot
    {
        get => itemSlot;    //수정확인
        set
        {
            itemSlot = value;            
            itemSlot.onSlotItemChange = Refresh;    // SlotUI에 Slot이 할당되면 Slot의 델리게이트에 할당된 함수를 이 인스턴스의 Refresh함수로 교체
            itemSlot.onEquipSlotChange = EquipRefresh;            
        }
    }    

    /// <summary>
    /// 이 UI에서 사용하고 있는 ItemData
    /// </summary>
    public ItemData SlotItemData
    {
        get => itemSlot.SlotItemData;
    }

    /// <summary>
    /// 이 슬롯UI에 들어있는 아이템 개수
    /// </summary>
    public uint ItemCount
    {
        get => itemSlot.ItemCount;
    }
    /// <summary>
    /// 슬롯의 식별용 번호. 인벤토리UI의 몇번째 슬롯인지 표시. 0번부터 시작
    /// </summary>
    public int ID { get => id; set => id = value; }



    // 함수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 보여지는 슬롯의 정보를 지금 가지고 있는 itemSlot의 데이타를 이용해 갱신
    /// </summary>
    public void Refresh()
    {
        if (itemSlot.SlotItemData != null) //현재 슬롯에 아이템이 있을 때
        {
            itemImage.sprite = itemSlot.SlotItemData.itemImage; // 이미지 변경
            itemImage.color = Color.white;                  // Alpha를 1로
            itemCountText.text = $"{itemSlot.ItemCount}";
        }
        else    //현재 슬롯에 아이템이 없을 때
        {
            itemImage.sprite = null;        // 이미지 비우고
            itemImage.color = Color.clear;  // alpha를 0으로 
            itemCountText.text = "";
        }
    }

    void EquipRefresh()
    {
        if (itemSlot.SlotItemData != null) //현재 슬롯에 아이템이 있을 때
        {
            itemEquipText.gameObject.SetActive(itemSlot.ItemEquiped);
        }
        else
        {
            itemEquipText.gameObject.SetActive(false);
        }
    }

    // 유니티 이벤트 함수들-------------------------------------------------------------------------
    private void Awake()
    {
        // 캐싱용 컴포넌트 찾기
        itemImage = transform.Find("ItemImage").GetComponent<Image>();    
        itemCountText = transform.Find("ItemCount").GetComponent<Text>();
        itemEquipText = transform.Find("ItemEquip").GetComponent<Text>();

        // 인벤토리 오브젝트 찾기
        invenUI = transform.parent.parent.GetComponent<InventoryUI>();
    }

    void Start()
    {
        // 디테일창의 rect 미리 받아오기
        detailRect = invenUI.Detail.transform as RectTransform;        
    }


    // 디테일 창용 마우스 입력 이벤트들 -------------------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 슬롯 안에 마우스가 들어오면 디테일창 열기
        //Debug.Log($"Enter : {itemSlot.SlotItem}");
        if (itemSlot.SlotItemData != null)
        {
            invenUI.Detail.Open(itemSlot.SlotItemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 슬롯 안에서 마우스가 나가면 디테일창 닫기
        //Debug.Log($"Exit : {itemSlot.SlotItem}");
        if (itemSlot.SlotItemData != null)
        {
            invenUI.Detail.Close();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        // 슬롯 안에서 마우스가 움직이면 디테일창 움직이기
        if (!invenUI.Detail.isActiveAndEnabled)
        {
            // 만약 디테일창이 비활성화 되어있으면 다시 연다.
            invenUI.Detail.Open(itemSlot.SlotItemData); 
        }

        Vector2 mousePos = eventData.position;
        if ((mousePos.x + detailRect.sizeDelta.x) > Screen.width)
        {
            mousePos.x -= detailRect.sizeDelta.x;       // 디테일창이 화면 밖으로 나가면 디테일창의 오른쪽 아래가 기준이 되도록 변경
        }

        invenUI.Detail.transform.position = mousePos;   // 마우스 위치에 맞춰 디테일창 이동
    }



    // 마우스 클릭(아이템 사용, 장비, 분리)용 함수 --------------------------------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)  // 왼쪽 마우스 클릭할 때만 처리
        {
            if (Keyboard.current.leftShiftKey.ReadValue() > 0.0f)   // 왼쪽 쉬프트키가 눌러져 있다면
            {
                //쉬프트 키가 눌러져있다.
                //Debug.Log($"{this.gameObject.name} 쉬프트 클릭");
                invenUI.Spliter.Open(this);
            }
            else
            {
                //일반 클릭일 때
                if(invenUI.MovingSlot.ItemSlot.SlotItemData != null)
                {
                    //invenUI.SplitItem(invenUI.MovingSlot.SplitStartID, (uint)this.id, invenUI.MovingSlot.SplitCount);
                    invenUI.SpliteItem_End((uint)this.id, invenUI.MovingSlot.SplitCount);
                    invenUI.MovingSlot.Close();
                }
                else
                {
                    // 아이템을 사용하는 상황
                    //Debug.Log($"{this.gameObject.name} 클릭");

                    // 플레이어를 대상으로 아이템을 사용하거나 장비
                    itemSlot.UseSlot(GameManager.Inst.MainPlayer.gameObject);
                    if (itemSlot.ItemCount <= 0)
                    {
                        // 사용 가능한 아이템이라 사용후 
                        invenUI.Detail.Close();
                    }
                }
            }
        }
    }
}
