using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI placeName;
    ConstructionLine vilaConstructionLine;
    ConstructionLine buildingConstructionLine;
    ConstructionLine hotelConstructionLine;
    TextMeshProUGUI totalBuildPrice;
    Button okButton;
    Button cancelButton;

    Player player;
    City targetCity;
    int totalPrice = 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        placeName = transform.Find("PlaceName").GetComponent<TextMeshProUGUI>();
        vilaConstructionLine = transform.Find("SetConstruction").Find("ConstructionLine_Vila").GetComponent<ConstructionLine>();
        buildingConstructionLine = transform.Find("SetConstruction").Find("ConstructionLine_Building").GetComponent<ConstructionLine>();
        hotelConstructionLine = transform.Find("SetConstruction").Find("ConstructionLine_Hotel").GetComponent<ConstructionLine>();
        totalBuildPrice = transform.Find("TotalBuildPrice").GetComponent<TextMeshProUGUI>();
        okButton = transform.Find("OKButton").GetComponent<Button>();
        cancelButton = transform.Find("CancelButton").GetComponent<Button>();

        vilaConstructionLine.OnTotalPriceChange += TotalBuildPriceChange;
        buildingConstructionLine.OnTotalPriceChange += TotalBuildPriceChange;
        hotelConstructionLine.OnTotalPriceChange += TotalBuildPriceChange;

        okButton.onClick.AddListener(OnOkClick);
        cancelButton.onClick.AddListener(OnCancleClick);
    }

    public void Show(bool isShow, Player arrived, City city)
    {
        if (isShow)
        {
            // UI를 보여줄 때
            player = arrived;
            targetCity = city;

            placeName.text = city.placeName;
            vilaConstructionLine.Initialize(city.buildingDatas[(int)BuildingType.Villa].price);
            buildingConstructionLine.Initialize(city.buildingDatas[(int)BuildingType.Building].price);
            hotelConstructionLine.Initialize(city.buildingDatas[(int)BuildingType.Hotel].price);
            totalBuildPrice.text = "0";

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            // UI를 끌 때
            player.PlayerTurnEnd();
            player = null;
            targetCity = null;

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    void OnOkClick()
    {
        int[] counts = new[]{ vilaConstructionLine.Count, buildingConstructionLine.Count, hotelConstructionLine.Count };
        targetCity.MakeBuildings(counts);

        Show(false, null, null);
    }

    void OnCancleClick()
    {
        Show(false, null, null);
    }

    void TotalBuildPriceChange()
    {
        totalPrice = vilaConstructionLine.TotalPrice + buildingConstructionLine.TotalPrice + hotelConstructionLine.TotalPrice;
        totalBuildPrice.text = totalPrice.ToString();
        okButton.interactable = (player.Money >= totalPrice);        
    }
}
