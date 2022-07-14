using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CityBase
{
    public BuildingData[] buildingDatas;
    //GameObject[] buildingObj;

    protected override void Awake()
    {
        base.Awake();

        int NumOfBuildingTypes = System.Enum.GetValues(typeof(BuildingType)).Length;
        buildingDatas = new BuildingData[NumOfBuildingTypes];
        //buildingObj = new GameObject[NumOfBuildingTypes];
        //for (int i = 0; i < NumOfBuildingTypes; i++)
        //{
        //    buildingObj[i] = transform.GetChild(i+1).gameObject;
        //    buildingObj[i].SetActive(false);
        //}
    }

    public void MakeBuildings(int[] counts)
    {
        //buildingObj[(int)type].SetActive(true);
        Player ownerPlayer = GameManager.Inst.Players[(int)owner];

        for (int i = 0; i < counts.Length; i++)
        {
            buildingDatas[i].count += counts[i];
            
            int totalPrice = buildingDatas[i].price * counts[i];
            ownerPlayer.Money -= totalPrice;
        }
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

        totalValue = buildingValue;
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

    public override void Initialize(GameObject obj, ref MapData mapData)
    {
        base.Initialize(obj, ref mapData);
        buildingDatas[(int)BuildingType.Villa].price = mapData.villaBuyPrice;
        buildingDatas[(int)BuildingType.Villa].usePrice = mapData.villaUsePrice;
        buildingDatas[(int)BuildingType.Villa].count = 0;
        buildingDatas[(int)BuildingType.Building].price = mapData.buildingBuyPrice;
        buildingDatas[(int)BuildingType.Building].usePrice = mapData.buildingUsePrice;
        buildingDatas[(int)BuildingType.Building].count = 0;
        buildingDatas[(int)BuildingType.Hotel].price = mapData.hotelBuyPrice;
        buildingDatas[(int)BuildingType.Hotel].usePrice = mapData.hotelUsePrice;
        buildingDatas[(int)BuildingType.Hotel].count = 0;
    }

    public override void OnArrive(Player player)
    {
        if(player.Type == Owner)
        {
            // 건물 짓기
            GameManager.Inst.UI_Manager.ShowBuildingPanel(true, player, this);
        }
        else
        {
            base.OnArrive(player);
        }
    }
}
