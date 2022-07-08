using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    DiceRollPanel diceRollPanel;
    ResultPanel resultPanel;

    public void Initialize()
    {
        diceRollPanel = FindObjectOfType<DiceRollPanel>();
        resultPanel = FindObjectOfType<ResultPanel>();
    }

    public void ShowDiceRollPanel(bool isShow)
    {
        diceRollPanel.Show(isShow);
    }

    public void SetResultText(PlayerType type, int diceEyes)
    {
        resultPanel.SetText(type, diceEyes);
    }
}
