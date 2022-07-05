using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_FundGet : Place
{
    int totalFund = 0;

    public void AddFund(int money)
    {
        totalFund += money;
    }

    public override void OnArrive(Player player)
    {
        player.Money += totalFund;
        totalFund = 0;
    }
}
