using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// 행동 결과창
/// </summary>
public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI textUI;
    bool isDouble = false;

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
        if (isDouble)
        {
            textUI.text = $"{type}의 주사위는 {diceEyes}(이)가 나왔습니다.[더블]";
            isDouble = false;
        }
        else
        {
            textUI.text = $"{type}의 주사위는 {diceEyes}(이)가 나왔습니다.";
        }
        //Debug.Log(textUI.text);
    }

    /// <summary>
    /// 더블이 나왔을 때 실행될 함수. 델리게이터에 간접적으로 연결
    /// </summary>
    /// <param name="_">사용안함</param>
    public void OnDouble(PlayerType _)
    {
        isDouble = true;
    }
}
