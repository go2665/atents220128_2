using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 돈 표시 판
/// </summary>
public class MoneyPanel : MonoBehaviour
{
    TextMeshProUGUI[] moneyText;
    TextMeshProUGUI[] unitText;

    private void Awake()
    {
        moneyText = new TextMeshProUGUI[transform.childCount];
        unitText = new TextMeshProUGUI[transform.childCount];
        for (int i=0;i<transform.childCount;i++)
        {
            moneyText[i] = transform.GetChild(i).Find("Money").GetComponent<TextMeshProUGUI>();
            unitText[i] = transform.GetChild(i).Find("Unit").GetComponent<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// 특정 플레이어의 돈 글자 변경.
    /// 각 플레이어의 OnMoneyChange 델리게이트에 연결이 되어 각 플레이어의 보유금액이 변경되면 자동 실행
    /// </summary>
    /// <param name="type">돈이 변경된 플레이어</param>
    /// <param name="player">돈이 변경된 </param>
    public void SetMoneyText(Player player)
    {
        int index = (int)player.Type - 1;

        if (player.Money >= 0)
        {
            moneyText[index].text = player.Money.ToString("N0");
            unitText[index].text = "만원";
        }
        else
        {
            moneyText[index].text = "파산";
            unitText[index].text = "";
        }
    }
}
