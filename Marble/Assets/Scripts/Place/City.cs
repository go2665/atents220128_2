using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CityBase
{
    public BuildingData[] buildings;

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
    }

    void ReCalcValue()
    {
        int buildingValue = 0;
        foreach (var b in buildings)
        {
            buildingValue += b.price * b.count;
        }

        value = price + buildingValue;
    }
}
