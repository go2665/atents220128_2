using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CityBaseBuyPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI placeName;
    TextMeshProUGUI question;
    Button yesButton;
    Button noButton;
    BuyDataLine dataLine;

    Player player;
    CityBase targetCity;

    readonly string buyQuestionstr = "구매하시겠습니까?";
    readonly string lackOfMoenyStr = "금액이 부족합니다.";

    private void Awake()
    {
        // UI요소들 가져오기
        canvasGroup = GetComponent<CanvasGroup>();
        placeName = transform.Find("PlaceName").GetComponent<TextMeshProUGUI>();
        question = transform.Find("Question").GetComponent<TextMeshProUGUI>();
        yesButton = transform.Find("YesButton").GetComponent<Button>();
        noButton = transform.Find("NoButton").GetComponent<Button>();
        yesButton.onClick.AddListener(OnClickYes);
        noButton.onClick.AddListener(OnClickNo);
        dataLine = GetComponentInChildren<BuyDataLine>();
    }

    /// <summary>
    /// CityBaseBuyPanel을 표시하거나 끄는 함수
    /// </summary>
    /// <param name="isShow">부여줄지 여부. true면 보여준다.</param>
    /// <param name="arrived">도시에 도착한 플레이어</param>
    /// <param name="city">도착한 도시</param>
    public void Show(bool isShow, Player arrived, CityBase city)
    {
        if (isShow)
        {
            // UI를 보여줄 때
            player = arrived;
            targetCity = city;

            placeName.text = city.placeName;    // 도시 이름 설정
            dataLine.SetData(city.price, city.usePrice);    // 도시 가격 표시
            if (city.CanBuy(arrived.Type))      // 구매 가능한지 확인
            {
                // 살 수 있으면 구매버튼 활성화
                yesButton.interactable = true;
                question.text = buyQuestionstr;
            }
            else
            {
                // 살 수 없으면 구매버튼 비활성화
                yesButton.interactable = false;
                question.text = lackOfMoenyStr;
            }

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;            
        }
        else
        {
            // UI를 끌 때
            player = null;
            targetCity = null;

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void OnClickYes()
    {
        Debug.Log("산다.");
        targetCity.Sell(player.Type);   // 해당 도시 구매 실행
        
        PanelEnd(); // 종료
    }

    private void OnClickNo()
    {
        Debug.Log("안산다.");
        PanelEnd(); // 종료
    }

    /// <summary>
    /// 플레이어의 턴을 종료하고 패널을 보이지 않게 만들기
    /// </summary>
    void PanelEnd()
    {
        player.PlayerTurnEnd();
        Show(false, null, null);
    }

}
