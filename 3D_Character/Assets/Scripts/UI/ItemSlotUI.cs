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

                    if( ItemSlot.SlotItem == null )
                    {
                        // 비어있는 슬롯인 경우
                        //Debug.Log("비어있는 슬롯");
                        itemSlot.AssignSlotItem(invenUI.SplittedItem.ItemSlot.SlotItem, invenUI.SplittedItem.ItemCount);
                        invenUI.SplittedItem.Close();
                    }
                    else
                    {
                        // 아이템이 들어있는 슬롯인 경우
                        //Debug.Log("아이템이 있는 슬롯");
                        if(itemSlot.SlotItem == invenUI.SplittedItem.ItemSlot.SlotItem)
                        {
                            // 같은 종류의 아이템이다.
                            //Debug.Log("같은 종류의 아이템이 있는 슬롯");

                            if(itemSlot.SlotItem.maxStackCount < (itemSlot.ItemCount + invenUI.SplittedItem.ItemCount))
                            {
                                // 새 슬롯에 있는 개수 + 분리된 아이템 개수가 최대치를 넘겼을 때
                                // 최대 수치를 넘김
                                //Debug.Log("개수가 넘쳤다.");
                                if (itemSlot.ItemCount < itemSlot.SlotItem.maxStackCount)
                                {
                                    int overCount = itemSlot.ItemCount + invenUI.SplittedItem.ItemCount - itemSlot.SlotItem.maxStackCount;
                                    itemSlot.IncreaseSlotItem(overCount);           // 대상에 남은 공간만큼 채우고
                                    invenUI.SplittedItem.ItemCount -= overCount;    // 그 나머지는 그대로 SplittedItem에 남아있도록 처리
                                }
                            }
                            else
                            {
                                // 합이 최대치보다 작거나 같을 때
                                //Debug.Log("그냥 합치면 된다.");
                                itemSlot.IncreaseSlotItem(invenUI.SplittedItem.ItemCount);  // 그냥 ItemCount만큼 더하기
                                invenUI.SplittedItem.Close();
                            }
                        }
                        else
                        {
                            // 다른 종류의 아이템이다.
                            //Debug.Log("다른 종류의 아이템이 있는 슬롯");

                            // 일단은 무시
                        }
                    }                    
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
