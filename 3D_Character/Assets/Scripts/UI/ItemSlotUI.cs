using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ItemSlot을 표시해주는 클래스
/// </summary>
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private DetailInfoUI detail = null;
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
        detail = transform.parent.parent.Find("DetailInfo").GetComponent<DetailInfoUI>();
        detailRect = detail.transform as RectTransform;
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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{this.gameObject.name} 클릭");
            itemSlot.UseItem();
            if (itemSlot.ItemCount <= 0)
            {
                detail.Close();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"Enter : {itemSlot.SlotItem}");
        if (itemSlot.SlotItem != null)
        {
            detail.Open(itemSlot.SlotItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"Exit : {itemSlot.SlotItem}");
        if (itemSlot.SlotItem != null)
        {
            detail.Close();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {        
        Vector2 mousePos = eventData.position;
        if( (mousePos.x + detailRect.sizeDelta.x) > Screen.width )
        {
            mousePos.x -= detailRect.sizeDelta.x;
        }

        detail.transform.position = mousePos;
    }
}
