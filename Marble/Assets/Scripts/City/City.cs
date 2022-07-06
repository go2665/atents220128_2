using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CityBase
{
    public BuildingData[] buildingDatas;
    int totalUsePrice = 0;
    GameObject[] buildingObj;

    private void Awake()
    {
        int NumOfBuildingTypes = System.Enum.GetValues(typeof(BuildingType)).Length;
        buildingDatas = new BuildingData[NumOfBuildingTypes];
        //buildingObj = new GameObject[NumOfBuildingTypes];
        //for (int i = 0; i < NumOfBuildingTypes; i++)
        //{
        //    buildingObj[i] = transform.GetChild(i+1).gameObject;
        //    buildingObj[i].SetActive(false);
        //}
    }

    public void MakeBuilding(BuildingType type, int count)
    {
        buildingObj[(int)type].SetActive(true);
        buildingDatas[(int)type].count += count;

        Player ownerPlayer = GameManager.Inst.Players[(int)owner];
        ownerPlayer.Money -= buildingDatas[(int)type].price * count;

        ReCalcValue();
        ReCalcTotalUsePrice();
    }

    void ReCalcValue()
    {
        int buildingValue = price;
        foreach (var b in buildingDatas)
        {
            buildingValue += b.price * b.count;
        }

        value = buildingValue;
    }

    void ReCalcTotalUsePrice()
    {
        int total = usePrice;
        foreach (var b in buildingDatas)
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

    public override void Initialize(GameObject obj, ref MapData mapData)
    {
        base.Initialize(obj, ref mapData);
        buildingDatas[(int)BuildingType.Villa].price = mapData.villaBuyPrice;
        buildingDatas[(int)BuildingType.Villa].usePrice = mapData.villaUsePrice;
        buildingDatas[(int)BuildingType.Building].price = mapData.buildingBuyPrice;
        buildingDatas[(int)BuildingType.Building].usePrice = mapData.buildingUsePrice;
        buildingDatas[(int)BuildingType.Hotel].price = mapData.hotelBuyPrice;
        buildingDatas[(int)BuildingType.Hotel].usePrice = mapData.hotelUsePrice;
    }
}
