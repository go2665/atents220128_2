using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Test_Dice : MonoBehaviour
{
    public TextMeshProUGUI dice1;
    public TextMeshProUGUI dice2;
    public TextMeshProUGUI doubleText;
    public TextMeshProUGUI diceSum;
    public Button rollButton;

    void Start()
    {
        Debug.Log("주사위를 하나씩 굴립니다.");        
        for(int i=0;i< GameManager.Inst.GameDiceSet.NumOfDices; i++)
        {
            Debug.Log($"Dice_{i}(은)는 {GameManager.Inst.GameDiceSet.Roll(i)}(이)가 나왔습니다.");
        }

        rollButton.onClick.AddListener(DiceRoll);
    }

    private void DiceRoll()
    {
        bool isDouble = GameManager.Inst.GameDiceSet.RollAll(out int[] result);
        PrintDiceResult(result[0], result[1], isDouble);
        PrintDiceSumResult(GameManager.Inst.GameDiceSet.RollAll());
    }

    void PrintDiceResult(int d1, int d2, bool isDouble)
    {
        dice1.text = d1.ToString();
        dice2.text = d2.ToString();
        if( isDouble)
        {
            doubleText.text = "[더블]";
        }
        else
        {
            doubleText.text = "";
        }
    }

    void PrintDiceSumResult(int sum)
    {
        diceSum.text = sum.ToString();
    }
}
