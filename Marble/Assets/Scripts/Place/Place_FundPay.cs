using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_FundPay : Place
{
    int fundPrice = 150;

    void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.None);
    }

    public override void OnArrive(Player player)
    {
        player.Money -= fundPrice;
        Place_FundGet fundGet = GameManager.Inst.GameMap.GetPlace(MapID.Fund_Get) as Place_FundGet;
        if( fundGet != null )
        {
            fundGet.AddFund(fundPrice);
        }
        Debug.Log($"{player} : 기금에 {fundPrice}원을 지불합니다.");
    }
}
