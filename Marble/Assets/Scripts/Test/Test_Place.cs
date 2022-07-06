using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test_Place : MonoBehaviour
{
    public GameObject testObj;

    private void Start()
    {
        MapData testPlace = new();
        testPlace.id = 999;
        testPlace.type = PlaceType.City;
        testPlace.name = "테스트도시";
        testPlace.placeBuyPrice = 150;
        testPlace.placeUsePrice = 50;
        testPlace.villaBuyPrice = 100;
        testPlace.villaUsePrice = 200;
        testPlace.buildingBuyPrice = 300;
        testPlace.buildingUsePrice = 400;
        testPlace.villaBuyPrice = 500;
        testPlace.villaUsePrice = 600;

        City city = testObj.AddComponent<City>();
        city.Initialize(testObj, ref testPlace);
    }
}
