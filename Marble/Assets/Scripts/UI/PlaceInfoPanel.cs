using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceInfoPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI placeName;
    TextMeshProUGUI ownerName;
    TextMeshProUGUI vilaCount;
    TextMeshProUGUI buildingCount;
    TextMeshProUGUI hotelCount;
    TextMeshProUGUI totalValue;
    TextMeshProUGUI totalUsePrice;

    GameObject buildInfo;
    GameObject valueInfo;
    GameObject priceInfo;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        placeName = transform.Find("Names").Find("PlaceName").GetComponent<TextMeshProUGUI>();
        ownerName = transform.Find("Names").Find("OwnerName").GetComponent<TextMeshProUGUI>();
        buildInfo = transform.Find("BuildInfo").gameObject;
        vilaCount = buildInfo.transform.Find("VilaCount").GetComponent<TextMeshProUGUI>();
        buildingCount = buildInfo.transform.Find("BuildingCount").GetComponent<TextMeshProUGUI>();
        hotelCount = buildInfo.transform.Find("HotelCount").GetComponent<TextMeshProUGUI>();
        valueInfo = transform.Find("ValueInfo").gameObject;
        totalValue = valueInfo.transform.Find("TotalValue").GetComponent<TextMeshProUGUI>();
        priceInfo = transform.Find("PriceInfo").gameObject;
        totalUsePrice = priceInfo.transform.Find("TotalUsePrice").GetComponent<TextMeshProUGUI>();
    }

    public void SetInfo(MapID id)
    {
        SetInfo(GameManager.Inst.GameMap.GetPlace(id));
    }

    public void SetInfo(Place place)
    {
        if (place != null)
        {
            canvasGroup.alpha = 1;
            placeName.text = $"[{place.placeName}]";

            if (place.Type == PlaceType.City || place.Type == PlaceType.CityBase)
            {
                CityBase cityBase = (CityBase)place;
                ownerName.text = cityBase.Owner.ToString();
                buildInfo.SetActive(false);
                valueInfo.SetActive(true);
                priceInfo.SetActive(true);
                totalValue.text = cityBase.TotalValue.ToString();
                totalUsePrice.text = cityBase.TotalUsePrice.ToString();

                if (place.Type == PlaceType.City)
                {
                    City city = (City)place;
                    vilaCount.text = city.buildingDatas[(int)BuildingType.Villa].count.ToString();
                    buildingCount.text = city.buildingDatas[(int)BuildingType.Building].count.ToString();
                    hotelCount.text = city.buildingDatas[(int)BuildingType.Hotel].count.ToString();
                    buildInfo.SetActive(true);
                }
            }
            else
            {
                if(place.Type == PlaceType.SpaceShip)
                {
                    CityBase cityBase = GameManager.Inst.GameMap.GetPlace(MapID.Columbia) as CityBase;
                    ownerName.text = cityBase.Owner.ToString();
                }
                else
                {
                    ownerName.text = "-";
                }
                buildInfo.SetActive(false);
                valueInfo.SetActive(false);
                priceInfo.SetActive(false);
            }
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }
}
