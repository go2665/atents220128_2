using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailInfoUI : MonoBehaviour
{
    Text itemName = null;
    Image itemIcon = null;
    Text itemDesc = null;
    Text itemPrice = null;

    //데이터
    ItemData itemData = null;

    private void Awake()
    {
        itemName = transform.Find("Name").GetComponent<Text>();
        itemIcon = transform.Find("Icon").GetComponent<Image>();
        itemDesc = transform.Find("Desc").GetComponent<Text>();
        itemPrice = transform.Find("Price").GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.Inst.InventoryUI.onInventoryClose += Close; // 디테일 창이 떠있는체로 인벤토리가 닫히면 다시 열렸을 때 그대로 남아있는 버그 수정
        this.gameObject.SetActive(false);
    }

    //기능
    void Refresh()
    {
        if(itemData != null)
        {
            itemName.text = itemData.itemName;
            itemIcon.sprite = itemData.itemImage;
            itemDesc.text = itemData.itemDesc;
            itemPrice.text = $"가격 : {itemData.price} 골드";
        }
    }

    public void Open(ItemData data)
    {
        itemData = data;
        Refresh();
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        itemData = null;
    }
}
