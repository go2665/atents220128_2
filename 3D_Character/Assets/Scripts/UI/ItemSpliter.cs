using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpliter : MonoBehaviour
{
    public int itemSplitCount = 1;
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
    }

    void OnInputChage(string change)
    {
    }

    void IncreaseCount()
    {

    }

    void DecreaseCount()
    {

    }

}
