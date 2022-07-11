using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Place_FundGet : Place
{
    public GameObject fundTextPrefab;
    TextMeshPro fundText;
    int totalFund = 0;

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Fund);
        cover.transform.Rotate(0, 0, 90);        
    }

    private void Start()
    {
        GameObject textObj = Instantiate(fundTextPrefab, this.transform);
        fundText = textObj.GetComponent<TextMeshPro>();
    }

    public void AddFund(int money)
    {
        totalFund += money;
        fundText.text = totalFund.ToString();
    }

    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : 기금을 {totalFund}원 얻었습니다.");
        player.Money += totalFund;
        totalFund = 0;
        fundText.text = totalFund.ToString();
    }
}
