using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using System;

public class RPS_GameManager : MonoBehaviour
{
    Button rock;
    Button paper;
    Button scissors;
    TextMeshProUGUI result;
    TextMeshProUGUI mySelection;
    TextMeshProUGUI oppenentSelection;

    RPS_Player player = null;
    public RPS_Player Player
    {
        get => player;
        set
        {
            if(player == null)
            {
                player = value;
            }
        }
    }

    RPS_Player enemy = null;
    public RPS_Player Enemy
    {
        get => enemy;
        set
        {
            if (enemy == null)
            {
                enemy = value;
            }
        }
    }


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

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(480, 10, 150, 150));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host"))
                NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client"))
                NetworkManager.Singleton.StartClient();
        }
        GUILayout.EndArea();
    }

    void Initialize()
    {
        rock = GameObject.Find("Button_Rock").GetComponent<Button>();
        paper = GameObject.Find("Button_Paper").GetComponent<Button>();
        scissors = GameObject.Find("Button_Scissors").GetComponent<Button>();
        result = GameObject.Find("ResultText").GetComponent<TextMeshProUGUI>();
        mySelection = GameObject.Find("MySelection").GetComponent<TextMeshProUGUI>();
        oppenentSelection = GameObject.Find("OppenentSelection").GetComponent<TextMeshProUGUI>();

        rock.onClick.AddListener(() => OnHandClick(HandSelection.Rock));
        paper.onClick.AddListener(() => OnHandClick(HandSelection.Paper));
        scissors.onClick.AddListener(() => OnHandClick(HandSelection.Scissors));
    }

    private void OnHandClick(HandSelection hand)
    {
        if (player && player.Selection == HandSelection.None)
        {
            player.Selection = hand;
            switch (hand)
            {                
                case HandSelection.Rock:
                    mySelection.text = "바위";
                    break;
                case HandSelection.Paper:
                    mySelection.text = "보";
                    break;
                case HandSelection.Scissors:
                    mySelection.text = "가위";
                    break;
                case HandSelection.None:
                default:
                    break;
            }            
        }
    }

    public void SetOpponentText(string text)
    {
        oppenentSelection.text = text;
    }

}
