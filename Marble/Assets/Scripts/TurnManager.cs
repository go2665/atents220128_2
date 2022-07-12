using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어들의 턴을 관리하는 메니저
/// </summary>
public class TurnManager : MonoBehaviour
{
    /// <summary>
    /// 플레이어 목록(은행 제거)
    /// </summary>
    Player[] playersOnly;

    /// <summary>
    /// 인간 플레이어
    /// </summary>
    Player human;

    /// <summary>
    /// 현재 턴을 진행중인 플레이어
    /// </summary>
    PlayerType currentPlayer = PlayerType.Bank;
    public PlayerType CurrentPlayer
    {
        get => currentPlayer;
    }

    /// <summary>
    /// 다음 플레이어를 가져오는 프로퍼티(가지고 올 때 currentPlayer도 변경됨)
    /// </summary>
    public Player NextPlayer
    {
        get
        {
            if (playersOnly[(int)currentPlayer-1].ActionDone)
            {
                if (currentPlayer != PlayerType.CPU3)   // Human->CPU1->CPU2->CPU3->Human->...
                {
                    currentPlayer = currentPlayer + 1;
                }
                else
                {
                    currentPlayer = PlayerType.Human;
                }
            }
            return playersOnly[(int)(currentPlayer) - 1];
        }
    }

    /// <summary>
    /// 초기화 함수(반드시 플레이어가 초기화 된 후에 실행되어야 한다.)
    /// </summary>
    public void Initialize()
    {
        int num = GameManager.Inst.NumOfPlayer - 1;
        playersOnly = new Player[num];
        for (int i = 0; i < num; i++)
        {
            playersOnly[i] = GameManager.Inst.Players[i + 1];
        }
        human = playersOnly[0];
    }

    /// <summary>
    /// 최초의 턴을 시작하는 함수
    /// </summary>
    public void GameStart()
    {
        currentPlayer = PlayerType.Human;
        human.PlayerTurnStart();
    }
}
