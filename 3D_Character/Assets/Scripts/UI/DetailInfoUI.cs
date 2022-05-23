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
