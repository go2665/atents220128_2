using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    DiceRollPanel diceRollPanel;
    ResultPanel resultPanel;
    MoneyPanel moneyPanel;

    public void Initialize()
    {
        diceRollPanel = FindObjectOfType<DiceRollPanel>();
        resultPanel = FindObjectOfType<ResultPanel>();
        moneyPanel = FindObjectOfType<MoneyPanel>();

        GameManager.Inst.GameDiceSet.OnDouble += resultPanel.OnDouble;

        // 람다식을 이렇게 쓰면 안된다.
        //for (int i = 1; i < GameManager.Inst.NumOfPlayer; i++)
        //{
        //    GameManager.Inst.Players[i].OnMoneyChange +=
        //        (money) => moneyPanel.SetMoneyText((PlayerType)i, money);
        //}

        GameManager.Inst.GetPlayer(PlayerType.Human).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.Human, money);
        GameManager.Inst.GetPlayer(PlayerType.CPU1).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.CPU1, money);
        GameManager.Inst.GetPlayer(PlayerType.CPU2).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.CPU2, money);
        GameManager.Inst.GetPlayer(PlayerType.CPU3).OnMoneyChange +=
                (money) => moneyPanel.SetMoneyText(PlayerType.CPU3, money);
    }

    public void ShowDiceRollPanel(bool isShow)
    {
        diceRollPanel.Show(isShow);
    }

    public void SetResultText(PlayerType type, int diceEyes)
    {
        resultPanel.SetText(type, diceEyes);
    }

    public void SetResultText(string str)
    {
        resultPanel.SetText(str);
    }
}
