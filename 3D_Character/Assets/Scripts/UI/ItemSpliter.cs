using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpliter : MonoBehaviour
{
    public int itemSplitCount = 1;
    public int ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            itemSplitCount = value;
            itemSplitCount = Mathf.Max(1, itemSplitCount);
            if (slot != null)
            {
                itemSplitCount = Mathf.Min(itemSplitCount, slot.ItemCount - 1);
            }
            inputField.text = itemSplitCount.ToString();
        }
    }
    private ItemSlot slot = null;
    private InputField inputField = null;

    private void Awake()
    {
        inputField = transform.Find("InputField").GetComponent<InputField>();
        inputField.onValueChanged.AddListener(OnInputChage);
        Button increase = transform.Find("Button_Increase").GetComponent<Button>();
        increase.onClick.AddListener(IncreaseCount);
        Button decrease = transform.Find("Button_Decrease").GetComponent<Button>();
        decrease.onClick.AddListener(DecreaseCount);
        Button ok = transform.Find("Button_OK").GetComponent<Button>();
        ok.onClick.AddListener(SelectOK);
        Button cancel = transform.Find("Button_Cancel").GetComponent<Button>();
        cancel.onClick.AddListener(SelectCancel);
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ItemSplitCount = itemSplitCount;
    }

    void OnInputChage(string change)
    {
        ItemSplitCount = int.Parse(change);
    }

    void IncreaseCount()
    {
        ItemSplitCount++;
    }

    void DecreaseCount()
    {
        ItemSplitCount--;
    }

    void SelectOK()
    {
        this.gameObject.SetActive(false);
        // slot에 있는 아이템 아이콘이 보인다. -> 마우스를 따라간다.
            // -> 빈슬롯을 클릭 -> 그 슬롯에 들어간다.
            // -> 아이템이 있는 슬롯
                // -> 같은 종류의 아이템
                    // -> 공간이 충분하다.      -> 아이템을 합친다.
                    // -> 공간이 충분하지 않다. -> 무시
                // -> 다른 종류의 아이템        -> 무시

    }

    void SelectCancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Open(ItemSlot newSlot)
    {
        if (newSlot.ItemCount > 1)
        {
            slot = newSlot;
            this.gameObject.SetActive(true);
        }
    }

}
