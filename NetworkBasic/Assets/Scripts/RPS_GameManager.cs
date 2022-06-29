using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class RPS_GameManager : MonoBehaviour
{
    Button rock;
    Button paper;
    Button scissors;
    TextMeshProUGUI result;
    TextMeshProUGUI mySelection;
    TextMeshProUGUI oppenentSelection;

    private static RPS_GameManager instance = null;
    public static RPS_GameManager Inst
    {
        get => instance;
    }

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        rock = GameObject.Find("Button_Rock").GetComponent<Button>();
        paper = GameObject.Find("Button_Paper").GetComponent<Button>();
        scissors = GameObject.Find("Button_Scissors").GetComponent<Button>();
        result = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();
        mySelection = GameObject.Find("MySelection").GetComponent<TextMeshProUGUI>();
        oppenentSelection = GameObject.Find("OppenentSelection").GetComponent<TextMeshProUGUI>(); ;
    }
}
