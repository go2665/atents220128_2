using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test_Place : MonoBehaviour
{
    private void Start()
    {
        string path = $"{Application.dataPath}/data";
        string fullPath = $"{path}/MapData.csv";

        string[] allLineText = File.ReadAllLines(fullPath);
        MapData[] mapDatas = new MapData[allLineText.Length-1];
        for (int i=1;i<allLineText.Length;i++)
        {
            string[] data = allLineText[i].Split(',');
            MapData mapData = new();
            mapData.id = int.Parse(data[0]);
            mapData.type = (PlaceType)int.Parse(data[1]);
            mapData.name = data[2];
            switch (mapData.type)
            {
                case PlaceType.City:
                    mapData.placeBuyPrice = int.Parse(data[3]);
                    mapData.placeUsePrice = int.Parse(data[4]);
                    mapData.villaBuyPrice = int.Parse(data[5]);
                    mapData.buildingBuyPrice = int.Parse(data[6]);
                    mapData.hotelBuyPrice = int.Parse(data[7]);
                    mapData.villaUsePrice = int.Parse(data[8]);
                    mapData.buildingUsePrice = int.Parse(data[9]);
                    mapData.hotelUsePrice = int.Parse(data[10]);
                    break;
                case PlaceType.CityBase:
                    mapData.placeBuyPrice = int.Parse(data[3]);
                    mapData.placeUsePrice = int.Parse(data[4]);
                    break;
                case PlaceType.Start:
                case PlaceType.Island:
                case PlaceType.Fund_Get:
                case PlaceType.Fund_Pay:
                case PlaceType.SpaceShip:
                case PlaceType.GoldenKey:
                default:
                    break;
            }
            
            mapDatas[i - 1] = mapData;
        }

        int j = 0;
    }
}
