using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ItemSlot을 표시해주는 클래스
/// </summary>
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    private Image itemImage = null;     // 아이템의 이미지를 표시할 UI Image
    private ItemSlot itemSlot = null;   // 표시할 ItemSlot
    public ItemSlot ItemSlot 
    {
        set
        {
            itemSlot = value;
            itemSlot.onSlotItemChange = Refresh;    // SlotUI에 Slot이 할당되면 델리게이트에 Refresh함수 할당
        }
    }

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();    // 아이템의 이미지를 표시할 UI 찾아놓기
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
        }
        else    //현재 슬롯에 아이템이 없을 때
        {
            itemImage.sprite = null;        // 이미지 비우고
            itemImage.color = Color.clear;  // alpha를 0으로 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{this.gameObject.name} 클릭");
    }
}
