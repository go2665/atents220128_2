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
    Player NextPlayer
    {
        get
        {
            PlayerType next;
            if (currentPlayer != PlayerType.CPU3)   // Human->CPU1->CPU2->CPU3->Human->...
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
        TurnStart();
    }

    /// <summary>
    /// 각 플레이어 별로 턴이 시작될 때 실행되는 함수
    /// </summary>
    void TurnStart()
    {
        Player player = NextPlayer;
        TurnProcess(player);
    }
    
    /// <summary>
    /// 플레이어의 턴을 진행하는 함수
    /// </summary>
    /// <param name="player">턴을 진행할 플레이어</param>
    void TurnProcess(Player player)
    {
        if (!player.ActionDone)
        {
            if (player.Type != PlayerType.Human)
            {
                // CPU 플레이어들
                player.RollDice();
                TurnProcess(player);    // 재귀 호출 방식으로 플레이어가 액션을 할 수 있으면 계속 반복
            }
            else
            {
                // 인간 플레이어                
                GameManager.Inst.UI_Manager.ShowDiceRollPanel(true);    // 패널 열어서 주사위 굴리기
            }
        }
        else
        {
            // 모든 행동이 완료되면 턴 종료
            TurnEnd();
        }
    }

    /// <summary>
    /// 플레이어용 턴 처리 함수. 주사위 굴림판을 클릭했을 때 실행되는 함수
    /// </summary>
    public void PlayerTurnProcess()
    {
        //Debug.Log("Player Roll");

        human.RollDice();       // 주사위 굴리고    

        if(human.ActionDone)
        {
            TurnEnd();          // 행동력을 다 쓰면 턴 종료
        }
        else
        {
            GameManager.Inst.UI_Manager.ShowDiceRollPanel(true);    // 다시 주사위 굴림판 열기
        }
    }

    /// <summary>
    /// 각 플레이어별로 턴이 종료될 때 실행될 함수
    /// </summary>
    void TurnEnd()  
    {        
        GameManager.Inst.GetPlayer(CurrentPlayer).OnTurnEnd();  // 행동력을 1로 만들고 무인도 대기시간 감소
        //OnTurnEnd?.Invoke();

        StartCoroutine(TurnStartDelay());   // 다음 턴 시작을 잠시 딜레이 시킴
    }

    /// <summary>
    /// 턴 시작을 잠시 딜레이시키기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnStartDelay()
    {
        yield return new WaitForSeconds(0.1f);
        TurnStart();
    }
}
