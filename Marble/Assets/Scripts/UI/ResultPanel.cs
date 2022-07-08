using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        textUI.text = text;
    }

    internal void SetText(PlayerType type, int diceEyes)
    {
        textUI.text = $"{type}의 주사위는 {diceEyes}(이)가 나왔습니다.";
    }
}
