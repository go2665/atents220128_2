using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 구매금액과 사용료 표시하는 클래스
/// </summary>
public class BuyDataLine : MonoBehaviour
{
    TextMeshProUGUI buyPrice;
    TextMeshProUGUI usePrice;

    private void Awake()
    {
        buyPrice = transform.Find("BuyPrice").GetComponent<TextMeshProUGUI>();
        usePrice = transform.Find("UsePrice").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 구매 금액과 사용료를 텍스트로 표시
    /// </summary>
    /// <param name="buy">구매 금액</param>
    /// <param name="use">사용 금액</param>
    public void SetData(int buy, int use)
    {
        buyPrice.text = buy.ToString();
        usePrice.text = use.ToString();
    }
}
