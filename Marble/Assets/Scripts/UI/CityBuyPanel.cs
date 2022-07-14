using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CityBuyPanel : BuyPanel
{
    BuyDataLine vilaDataLine;
    BuyDataLine buildingDataLine;
    BuyDataLine hotelDataLine;

    protected override void Awake()
    {
        base.Awake();
        vilaDataLine = transform.Find("PlaceData").Find("VilaDataLine").GetComponent<BuyDataLine>();
        buildingDataLine = transform.Find("PlaceData").Find("BuildingDataLine").GetComponent<BuyDataLine>();
        hotelDataLine = transform.Find("PlaceData").Find("HotelDataLine").GetComponent<BuyDataLine>();
    }

    public override void Show(bool isShow, Player arrived, CityBase cityBase)
    {
        base.Show(isShow, arrived, cityBase);

        if (isShow)
        {
            City city = (City)cityBase;
            vilaDataLine.SetData(city.buildingDatas[(int)BuildingType.Villa].price, city.buildingDatas[(int)BuildingType.Villa].usePrice);
            buildingDataLine.SetData(city.buildingDatas[(int)BuildingType.Building].price, city.buildingDatas[(int)BuildingType.Building].usePrice);
            hotelDataLine.SetData(city.buildingDatas[(int)BuildingType.Hotel].price, city.buildingDatas[(int)BuildingType.Hotel].usePrice);
        }
    }
}
