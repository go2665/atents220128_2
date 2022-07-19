using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    DiceRollPanel diceRollPanel;    // 주사위 굴림판 UI
    ResultPanel resultPanel;        // 주사위 굴린 결과 창 UI(주사위의 눈 합, 더블 여부, 무인도 탈출 여부 출력)
    MoneyPanel moneyPanel;          // 각 플레이어들의 보유금액 표시
    PlaceInfoPanel placeInfoPanel;
    SpaceShipPanel spaceShipPanel;
    CityBaseBuyPanel cityBaseBuyPanel;
    CityBuyPanel cityBuyPanel;
    BuildingPanel buildingPanel;
    UseGoldenKeyPanel useGoldenKeyPanel;

    /// <summary>
    /// 초기화 함수. 주사위와 플레이어의 초기화 이후에 실행되어야 한다.
    /// </summary>
    public void Initialize()
    {
        diceRollPanel = FindObjectOfType<DiceRollPanel>();  // 각 UI 가져오기
        resultPanel = FindObjectOfType<ResultPanel>();
        moneyPanel = FindObjectOfType<MoneyPanel>();
        placeInfoPanel = FindObjectOfType<PlaceInfoPanel>();
        spaceShipPanel = FindObjectOfType<SpaceShipPanel>();
        cityBaseBuyPanel = FindObjectOfType<CityBaseBuyPanel>();
        cityBuyPanel = FindObjectOfType<CityBuyPanel>();
        buildingPanel = FindObjectOfType<BuildingPanel>();
        useGoldenKeyPanel = FindObjectOfType<UseGoldenKeyPanel>();

        GameManager.Inst.GameDiceSet.OnDouble += OnDouble_Result;   // 주사위가 더블이 나왔을 때 resultPanel에서 표시하기 위한 함수 등록

        // 람다식을 이렇게 쓰면 안된다.
        //for (int i = 1; i < GameManager.Inst.NumOfPlayer; i++)
        //{
        //    GameManager.Inst.Players[i].OnMoneyChange +=
        //        (money) => moneyPanel.SetMoneyText((PlayerType)i, money);
        //}

        // 플레이어의 보유금액이 변경되었을 때 실행될 함수들 등록
        GameManager.Inst.GetPlayer(PlayerType.Human).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.Human, money);
        GameManager.Inst.GetPlayer(PlayerType.CPU1).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.CPU1, money);
        GameManager.Inst.GetPlayer(PlayerType.CPU2).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.CPU2, money);
        GameManager.Inst.GetPlayer(PlayerType.CPU3).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.CPU3, money);
    }

    /// <summary>
    /// 주사위 굴림판을 보여줄지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 주사위 굴림판을 보여준다.</param>
    public void ShowDiceRollPanel(bool isShow)
    {
        diceRollPanel.Show(isShow);
    }

    /// <summary>
    /// 주사위 굴린 결과를 결과창에 글자로 출력하는 함수
    /// </summary>
    /// <param name="type">주사위를 굴린 플레이어</param>
    /// <param name="diceEyes">나온 주사위 눈의 합</param>
    public void SetResultText(PlayerType type, int diceEyes)
    {
        resultPanel.SetText(type, diceEyes);
    }

    /// <summary>
    /// 원하는 글자를 결과창에 출력하는 함수
    /// </summary>
    /// <param name="str"></param>
    public void SetResultText(string str)
    {
        resultPanel.SetText(str);
    }

    /// <summary>
    /// 결과창에 더블 여부를 출력하기 위해 데이터를 넘겨주는 함수
    /// </summary>
    /// <param name="_">사용안함</param>
    void OnDouble_Result(PlayerType _)
    {
        resultPanel.OnDouble(_);
    }

    public void ShowSpaceShipPanel(bool isShow, Player arrived)
    {
        spaceShipPanel.Show(isShow, arrived);
    }

    public void ShowBuyPanel(bool isShow, Player arrived, CityBase city)
    {
        if (city.Type == PlaceType.CityBase)
        {
            cityBaseBuyPanel.Show(isShow, arrived, city);
        }
        else if(city.Type == PlaceType.City)
        {
            cityBuyPanel.Show(isShow, arrived, city);
        }
    }

    public void ShowBuildingPanel(bool isShow, Player arrived, City city)
    {
        buildingPanel.Show(isShow, arrived, city);
    }

    public void ShowUseGoldenKeyPanel(bool isShow, Player user, GoldenKeyType goldenKey)
    {
        useGoldenKeyPanel.Show(isShow, user, goldenKey);
    }

    public void SetPlaceInfo(Place place)
    {
        placeInfoPanel.SetInfo(place);
    }

    public void SetPlaceInfo(MapID id)
    {
        placeInfoPanel.SetInfo(id);
    }
}
