using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public System.Action OnTurnEnd;

    Map map;
    DiceSet dice;
    Player[] playersOnly;   // 은행은 빠짐
    Player human;

    PlayerType currentPlayer = PlayerType.Bank;
    public PlayerType CurrentPlayer
    {
        get => currentPlayer;
    }

    Player NextPlayer
    {
        get
        {
            PlayerType next;
            if (currentPlayer != PlayerType.CPU3)
            {
                next = currentPlayer + 1;
            }
            else
            {
                next = PlayerType.Human;
            }
            currentPlayer = next;
            return playersOnly[(int)(currentPlayer) - 1];
        }
    }


    public void Initialize()
    {
        map = GameManager.Inst.GameMap;
        dice = GameManager.Inst.GameDiceSet;        
    }

    public void GameStart()
    {
        int num = GameManager.Inst.NumOfPlayer - 1;
        playersOnly = new Player[num];
        for (int i = 0; i < num; i++)
        {
            playersOnly[i] = GameManager.Inst.Players[i + 1];
        }
        human = playersOnly[0];

        TurnStart();
    }

    void TurnStart()
    {
        Player player = NextPlayer;
        TurnProcess(player);
    }
    
    void TurnProcess(Player player)
    {
        if (!player.ActionDone)
        {
            if (player.Type != PlayerType.Human)
            {
                player.RollDice();
                TurnProcess(player);
            }
            else
            {
                // 패널 열어서 주사위 굴리기
                GameManager.Inst.UI_Manager.ShowDiceRollPanel(true);
            }
        }
        else
        {
            TurnEnd();
        }     
    }

    public void PlayerTurnProcess()
    {
        //Debug.Log("Player Roll");

        human.RollDice();        

        if(human.ActionDone)
        {
            TurnEnd();
        }
        else
        {
            GameManager.Inst.UI_Manager.ShowDiceRollPanel(true);
        }
    }

    void TurnEnd()
    {
        OnTurnEnd?.Invoke();
        TurnStart();
    }

    //private void Update()
    //{
    //    if( !human.ActionDone )
    //    {
    //        // 인간 플레이어가 주사위를 던질 수 있다.
    //    }
    //    else
    //    {
    //        // 인간 플레이어는 행동을 다 했으니 CPU 플레이어가 행동을 한다.
    //    }
    //}

}
