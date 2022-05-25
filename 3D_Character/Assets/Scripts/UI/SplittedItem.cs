using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SplittedItem : MonoBehaviour
{
    Image itemIcon = null;
    ItemSlot itemSlot = null;
    public ItemSlot ItemSlot { get => itemSlot; }
    //ItemData itemData = null;
    //public ItemData ItemData { get => itemData; }
    public int ItemCount { get; set; }

    private void Awake()
    {
        itemIcon = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        if(itemSlot != null)
        {
            itemIcon.sprite = itemSlot.SlotItem.itemImage;
        }
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.transform.position = Mouse.current.position.ReadValue();
    }

    /// <summary>
    /// 이 게임 오브젝트를 활성화 할 때 사용하는 함수. (반드시 이 함수로만 진행되어야 함)
    /// </summary>
    /// <param name="newItemData"></param>
    public void Open(ItemSlot targetSlot, int count)
    {
        this.transform.position = Mouse.current.position.ReadValue();
        itemSlot = targetSlot;
        //itemData = newItemData;
        ItemCount = count;
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        itemSlot.DecreaseSlotItem(ItemCount);
        itemSlot = null;
        //itemData = null;
        this.gameObject.SetActive(false);
    }
}
