using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_FundPay : Place
{
    int fundPrice = 150;

    public override void OnArrive(Player player)
    {
        player.Money -= fundPrice;
        Place_FundGet fundGet = GameManager.Inst.GameMap.GetPlace(MapID.Fund_Get) as Place_FundGet;
        if( fundGet != null )
        {
            fundGet.AddFund(fundPrice);
        }
    }
}
