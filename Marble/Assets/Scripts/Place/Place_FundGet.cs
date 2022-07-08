using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_FundGet : Place
{
    int totalFund = 0;

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Fund);
        cover.transform.Rotate(0, 0, 90);
    }

    public void AddFund(int money)
    {
        totalFund += money;
    }

    public override void OnArrive(Player player)
    {
        player.Money += totalFund;
        totalFund = 0;

        Debug.Log($"{player} : 기금을 {totalFund}원 얻었습니다.");
    }
}
