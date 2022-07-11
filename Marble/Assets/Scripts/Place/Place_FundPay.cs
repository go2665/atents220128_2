using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_FundPay : Place
{
    int fundPrice = 150;
    Place_FundGet fundGet;

    void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.FundPay);
    }

    private void Start()
    {
        fundGet = GameManager.Inst.GameMap.GetPlace(MapID.Fund_Get) as Place_FundGet;
    }

    public override void OnArrive(Player player)
    {
        player.Money -= fundPrice;
        
        if( fundGet != null )
        {
            fundGet.AddFund(fundPrice);
        }
        //Debug.Log($"{player} : 기금에 {fundPrice}원을 지불합니다.");
    }
}
