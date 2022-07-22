using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BankruptcyPanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject sellCityLinePrefab;
    Button sellBankButton;
    CanvasGroup mainCanvasGroup;
    CanvasGroup subCanvasGroup;
    ScrollRect sellListScroll;
    TextMeshProUGUI totalText;

    Player bankruptPlayer;
    List<CityBase> sellList;

    private void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
        Transform subTransform = transform.Find("SellBankPanel");
        subCanvasGroup = subTransform.GetComponent<CanvasGroup>();
        totalText = subTransform.Find("Result").Find("TotalCitySellPriceText").GetComponent<TextMeshProUGUI>();
        sellBankButton = transform.Find("BankButton").GetComponent<Button>();
        sellBankButton.onClick.AddListener(OpenSellBankPanel);
        sellListScroll = GetComponentInChildren<ScrollRect>();
    }

    public void Show(bool isShow, Player target)
    {
        bankruptPlayer = target;

        if (isShow)
        {
            mainCanvasGroup.alpha = 1;
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            mainCanvasGroup.alpha = 0;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;

            if( target.Money < 0 )
            {
                // 게임 오버
                Debug.Log("GameOver");
            }
        }
        subCanvasGroup.alpha = 0;
        subCanvasGroup.interactable = false;
        subCanvasGroup.blocksRaycasts = false;
    }

    void AddSellList(List<CityBase> sellCities)
    {
        RectTransform rectContent = (RectTransform)sellListScroll.content.transform;
        int total = 0;
        float heightSum = 0;
        //판매 목록에 넣기
        foreach (var city in sellCities)
        {
            SellCityLine line = Instantiate(sellCityLinePrefab, sellListScroll.content).GetComponent<SellCityLine>();
            line.SetData(city);
            heightSum += ((RectTransform)(line.transform)).rect.height;
            total += (city.TotalValue >> 1);
        }
        rectContent.sizeDelta = new Vector2(0, heightSum);
        totalText.text = total.ToString("N0");
    }

    void ClearSellList()
    {
        int size = sellListScroll.content.childCount;
        List<Transform> dels = new List<Transform>(size);
        for(int i=0; i<size; i++)
        {
            dels.Add(sellListScroll.content.GetChild(i));
        }
        while(dels.Count > 0)
        {
            Transform t = dels[0];
            dels.RemoveAt(0);
            Destroy(t.gameObject);
        }
    }

    void OpenSellBankPanel()
    {
        ClearSellList();

        subCanvasGroup.alpha = 1;
        subCanvasGroup.interactable = true;
        subCanvasGroup.blocksRaycasts = true;

        sellList = bankruptPlayer.OnBankrupt(false);
        AddSellList(sellList);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        GameObject raycastObj = eventData.pointerCurrentRaycast.gameObject;
        if (raycastObj == subCanvasGroup.gameObject
            || raycastObj.transform.parent.parent.gameObject == subCanvasGroup.gameObject)
        {
            Debug.Log("Sub Click");
            foreach(var city in sellList)
            {
                city.Sell(PlayerType.Bank);
            }
            Show(false, bankruptPlayer);
        }
    }
}
