using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Canvas_Result : MonoBehaviour
{
    const string Victory = "당신은 승리했습니다.";
    const string Defeat = "당신은 패배했습니다.";

    TextMeshProUGUI resultText;

    private void Awake()
    {        
        resultText = transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
        int i = 0;
    }

    private void OnEnable()
    {
        if(resultText==null)
        {
            resultText = transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
        }
    }

    public void OnVictory()
    {
        resultText.text = Victory;
    }

    public void OnDefeat()
    {
        resultText.text = Defeat;
    }

}
