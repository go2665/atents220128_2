using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CityBase
{
    public BuildingData[] buildings;
    int totalUsePrice = 0;

    private void Awake()
    {
        buildings = new BuildingData[System.Enum.GetValues(typeof(BuildingType)).Length];        
    }

    public void MakeBuilding(BuildingType type, int count)
    {
        buildings[(int)type].count += count;

        Player ownerPlayer = GameManager.Inst.Players[(int)owner];
        ownerPlayer.Money -= buildings[(int)type].price * count;

        ReCalcValue();
        ReCalcTotalUsePrice();
    }

    void ReCalcValue()
    {
        int buildingValue = price;
        foreach (var b in buildings)
        {
            buildingValue += b.price * b.count;
        }

        value = buildingValue;
    }

    void ReCalcTotalUsePrice()
    {
        int total = usePrice;
        foreach (var b in buildings)
        {
            total += b.usePrice * b.count;
        }
        totalUsePrice = total;
    }

    public override void OnArrive(Player player)
    {
        if (owner == PlayerType.Bank)
        {
            // 은행 땅이다. => 구매 여부 확인
        }
        else if (owner != player.Type)
        {
            // 남의 땅이다.
            player.Money -= totalUsePrice;
        }
        else
        {
            // 내 땅이다. => 건물 짖기 UI
        }
    }
}
