using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellCityLine : MonoBehaviour
{
    TextMeshProUGUI cityName;
    TextMeshProUGUI cityPrice;
    TextMeshProUGUI citySellPrice;

    private void Awake()
    {
        cityName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cityPrice = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        citySellPrice = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void SetData(CityBase cityBase)
    {
        cityName.text = cityBase.placeName;
        cityPrice.text = cityBase.TotalValue.ToString();
        citySellPrice.text = (cityBase.TotalValue >> 1).ToString();
    }
}
