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
    private InventoryUI invenUI = null;
    private RectTransform detailRect = null;

    private Text itemText = null;
    private Image itemImage = null;     // 아이템의 이미지를 표시할 UI Image
    public Image ItemImage { get => itemImage; }
    private ItemSlot itemSlot = null;   // 표시할 ItemSlot
    public ItemSlot ItemSlot 
    {
        get => itemSlot;
        set
        {
            itemSlot = value;
            itemSlot.onSlotItemChange = Refresh;    // SlotUI에 Slot이 할당되면 델리게이트에 Refresh함수 할당
        }
    }

    private int id = 0;
    public int ID { get => id; set => id = value; }     // 몇번째 슬롯인가
        
    private void Awake()
    {
        itemImage = transform.Find("ItemImage").GetComponent<Image>();    // 아이템의 이미지를 표시할 UI 찾아놓기
        itemText = transform.Find("ItemCount").GetComponent<Text>();
        invenUI = transform.parent.parent.GetComponent<InventoryUI>();        
    }

    void Start()
    {
        detailRect = invenUI.Detail.transform as RectTransform;
    }

    /// <summary>
    /// 보여지는 아이템 이미지를 지금 가지고 있는 itemSlot의 데이타를 이용해 갱신
    /// </summary>
    public void Refresh()
    {
        if( itemSlot.SlotItem != null ) //현재 슬롯에 아이템이 있을 때
        {
            itemImage.sprite = itemSlot.SlotItem.itemImage; // 이미지 변경
            itemImage.color = Color.white;                  // Alpha를 1로
            itemText.text = $"{itemSlot.ItemCount}";
        }
        else    //현재 슬롯에 아이템이 없을 때
        {
            itemImage.sprite = null;        // 이미지 비우고
            itemImage.color = Color.clear;  // alpha를 0으로 
            itemText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)  // 왼쪽 마우스 클릭할 때만
        {
            if (Keyboard.current.leftShiftKey.ReadValue() > 0.0f)   // 왼쪽 쉬프트키가 눌러져 있다면
            {
                //쉬프트 키가 눌러져있다.
                Debug.Log($"{this.gameObject.name} 쉬프트 클릭");
                invenUI.Spliter.Open(ItemSlot);
            }
            else
            {
                //일반 클릭일 때
                if (invenUI.SplittedItem.isActiveAndEnabled)        // SplittedItem 게임 오브젝트가 활성화 되어있다면
                {
                    // 아이템 분리작업을 마무리 하는 상황
                    itemSlot.AssignSlotItem(invenUI.SplittedItem.ItemSlot.SlotItem, invenUI.SplittedItem.ItemCount);
                    invenUI.SplittedItem.Close();
                }
                else
                {
                    // 아이템을 사용하는 상황
                    Debug.Log($"{this.gameObject.name} 클릭");
                    itemSlot.UseItem();
                    if (itemSlot.ItemCount <= 0)
                    {
                        invenUI.Detail.Close();
                    }
                }                
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"Enter : {itemSlot.SlotItem}");
        if (itemSlot.SlotItem != null)
        {
            invenUI.Detail.Open(itemSlot.SlotItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"Exit : {itemSlot.SlotItem}");
        if (itemSlot.SlotItem != null)
        {
            invenUI.Detail.Close();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {        
        Vector2 mousePos = eventData.position;
        if( (mousePos.x + detailRect.sizeDelta.x) > Screen.width )
        {
            mousePos.x -= detailRect.sizeDelta.x;
        }

        invenUI.Detail.transform.position = mousePos;
    }
}
