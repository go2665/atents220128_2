using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UseGoldenKeyPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI useGoldenKeyName;
    TextMeshProUGUI question;
    Button yesButton;
    Button noButton;

    Player player;
    GoldenKeyType goldenKey;

    readonly string goldenKeyName1 = "무인도 탈출권";
    readonly string goldenKeyName2 = "우대권";

    private void Awake()
    {
        // UI요소들 가져오기
        canvasGroup = GetComponent<CanvasGroup>();
        useGoldenKeyName = transform.Find("UseGoldenKey").GetComponent<TextMeshProUGUI>();
        question = transform.Find("Question").GetComponent<TextMeshProUGUI>();
        yesButton = transform.Find("YesButton").GetComponent<Button>();
        noButton = transform.Find("NoButton").GetComponent<Button>();
        yesButton.onClick.AddListener(OnClickYes);
        noButton.onClick.AddListener(OnClickNo);
    }

    public void Show(bool isShow, Player user = null, GoldenKeyType useGoldenKey = GoldenKeyType.IslandEscapeTicket)
    {
        if (isShow)
        {
            // UI를 보여줄 때
            player = user;
            goldenKey = useGoldenKey;

            if (useGoldenKey == GoldenKeyType.IslandEscapeTicket)
            {
                useGoldenKeyName.text = goldenKeyName1;
            }
            else
            {
                useGoldenKeyName.text = goldenKeyName2;
            }            

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            // UI를 끌 때
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void OnClickYes()
    {
        Debug.Log("사용한다.");
        player.UseGoldenKey(goldenKey);
        if (goldenKey == GoldenKeyType.IslandEscapeTicket)
        {
            player.PlayerTurnStart();
        }
        else if(goldenKey == GoldenKeyType.FreePassTicket)
        {
            player.PlayerTurnEnd();
        }

        PanelEnd(); // 종료
    }

    private void OnClickNo()
    {
        Debug.Log("사용하지 않는다.");
        if (goldenKey == GoldenKeyType.IslandEscapeTicket)
        {
            player.PlayerRollProcess();
        }
        else if (goldenKey == GoldenKeyType.FreePassTicket)
        {
            CityBase city = GameManager.Inst.GameMap.GetPlace(player.Position) as CityBase;
            city.PayUsePrice(player);
            player.PlayerTurnEnd();
        }

        PanelEnd(); // 종료
    }

    /// <summary>
    /// 패널을 보이지 않게 만들기
    /// </summary>
    void PanelEnd()
    {
        // 무인도 탈출권은 사용하지 않더라도 주사위는 굴려야 함
        Show(false);
    }
}
