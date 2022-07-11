using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyPanel : MonoBehaviour
{
    TextMeshProUGUI[] moneyText;

    private void Awake()
    {
        moneyText = new TextMeshProUGUI[transform.childCount];
        for (int i=0;i<transform.childCount;i++)
        {
            moneyText[i] = transform.GetChild(i).Find("Money").GetComponent<TextMeshProUGUI>();
        }
    }

    public void SetMoneyText(PlayerType type, int money)
    {
        int index = (int)type - 1;        
        moneyText[index].text = money.ToString();        
    }
}
