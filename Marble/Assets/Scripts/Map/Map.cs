using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject placeNormal;
    public GameObject placeCorner;

    Place[] places;
    const int SideSize = 10;
    const int NumOfSize = 4;
    const int NumOfPlaces = SideSize * NumOfSize;    

    public void Initialize()
    {
        places = new Place[NumOfPlaces];

        MapData[] mapDatas = LoadMapData();
        GameObject[] mapObjects = MakeMapObject();
        for (int i = 0; i < NumOfPlaces; i++)
        {
            switch (mapDatas[i].type)
            {
                case PlaceType.City:
                    places[i] = mapObjects[i].AddComponent<City>();
                    break;
                case PlaceType.CityBase:
                    places[i] = mapObjects[i].AddComponent<CityBase>();
                    break;
                case PlaceType.Start:
                    places[i] = mapObjects[i].AddComponent<Place_Start>();
                    break;
                case PlaceType.Island:
                    places[i] = mapObjects[i].AddComponent<Place_Island>();
                    break;
                case PlaceType.Fund_Get:
                    places[i] = mapObjects[i].AddComponent<Place_FundGet>();
                    break;
                case PlaceType.Fund_Pay:
                    places[i] = mapObjects[i].AddComponent<Place_FundPay>();
                    break;
                case PlaceType.SpaceShip:
                    places[i] = mapObjects[i].AddComponent<Place_SpaceShip>();
                    break;
                case PlaceType.GoldenKey:
                    places[i] = mapObjects[i].AddComponent<Place_GoldenKey>();
                    break;
                default:
                    break;
            }
            places[i].Initialize(mapObjects[i], ref mapDatas[i]);
        }
    }

    public Place GetPlace(MapID id)
    {
        return places[(int)id];
    }

    public void Move(Player player, int count)
    {
        int dest = (int)player.Position + count;
        if(dest >= NumOfPlaces)
        {
            dest -= NumOfPlaces;
            Place_Start start = GetPlace(MapID.Start) as Place_Start;
            start.GetSalaray(player);
        }

        player.Position = (MapID)dest;
        Place place = GetPlace(player.Position);
        player.transform.position = place.GetPlayerPosition(player.Type);
        place.OnArrive(player);
    }

    public void Move(Player player, MapID mapID)
    {
        int move = mapID - player.Position;
        if( move < 0 )
        {
            move = NumOfPlaces + move;
        }
        Move(player, move);
    }

    MapData[] LoadMapData()
    {
        string fullPath = $"{Application.dataPath}/Data/MapData.csv";

        string[] allLineText = File.ReadAllLines(fullPath);
        MapData[] mapDatas = new MapData[allLineText.Length - 1];
        for (int i = 1; i < allLineText.Length; i++)
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

        return mapDatas;
    }

    GameObject[] MakeMapObject()
    {
        // 보이는 부분 배치하기
        GameObject[] allPaceObject = new GameObject[NumOfPlaces];

        GameObject prefab = null;
        Vector3 makeDir = Vector3.left;
        float makeAngle = 0.0f;
        Vector3 startPos = transform.position;
        for (int i = 0; i < NumOfSize; i++)
        {
            GameObject place = null;
            for (int j = 0; j < SideSize; j++)
            {
                if (j == 9)
                {
                    prefab = placeCorner;
                }
                else
                {
                    prefab = placeNormal;
                }
                place = Instantiate(prefab, this.transform);
                int id = 1 + i * SideSize + j;
                id %= 40;
                place.name = $"ID_{id}";

                makeAngle = 90.0f * i;
                place.transform.Rotate(0, makeAngle, 0);
                place.transform.Translate(j * makeDir + startPos, Space.World);

                if (id == 0)
                {
                    place.transform.SetAsFirstSibling();

                }
                allPaceObject[id] = place;
            }
            startPos = place.transform.position;
            makeDir = Quaternion.Euler(0, 90, 0) * makeDir;
        }

        return allPaceObject;
    }
}
