using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConstructionLine : MonoBehaviour
{
    int unitPrice = 0;
    int UnitPrice
    {
        get => unitPrice;
        set
        {
            if( unitPrice != value )
            {
                unitPrice = value;
                RefreshUI();
            }
        }
    }
    int count = 0;
    public int Count
    {
        get => count;
        private set
        {
            int clmapValue = Mathf.Clamp(value, 0, 9);
            if(count != clmapValue)
            {
                count = clmapValue;
                countInput.text = count.ToString();
                RefreshUI();
            }
        }
    }

    public int TotalPrice
    {
        get => unitPrice * count;
    }

    public System.Action OnTotalPriceChange;

    TextMeshProUGUI unitPriceText;
    Button decrease;
    Button increase;
    TMP_InputField countInput;
    TextMeshProUGUI totalPrice;

    private void Awake()
    {
        unitPriceText = transform.Find("UnitPrice").GetComponent<TextMeshProUGUI>();
        decrease = transform.Find("DecreaseButton").GetComponent<Button>();
        increase = transform.Find("IncreaseButton").GetComponent<Button>();
        countInput = GetComponentInChildren<TMP_InputField>();
        totalPrice = transform.Find("TotalPrice").GetComponent<TextMeshProUGUI>();
        decrease.onClick.AddListener(OnDecreaseClick);
        increase.onClick.AddListener(OnIncreaseClick);
        countInput.onValueChanged.AddListener(OnInputValueChage);

        countInput.onEndEdit.AddListener(OnInputEnd);
    }

    private void OnInputEnd(string input)
    {
        int num = 0;
        if (input.Length > 0)
        {
            num = int.Parse(input);
        }
        
        num = Mathf.Clamp(num, 0, 9);
        countInput.text = num.ToString();
        Count = num;
    }

    private void OnInputValueChage(string input)
    {
        int num = int.Parse(input);
        num = Mathf.Clamp(num, 0, 9);
        Count = num;
    }

    void OnIncreaseClick()
    {
        Count++;
    }

    void OnDecreaseClick()
    {
        Count--;
    }

    void RefreshUI()
    {
        unitPriceText.text = UnitPrice.ToString();
        totalPrice.text = (UnitPrice * Count).ToString();
        OnTotalPriceChange?.Invoke();
    }

    public void Initialize(int price)
    {
        UnitPrice = price;
        unitPriceText.text = price.ToString();
        Count = 0;
    }
}
