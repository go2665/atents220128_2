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
    Queue<Player> playersQueue;

    /// <summary>
    /// 현재 턴을 진행중인 플레이어
    /// </summary>
    Player currentPlayer = null;
    public Player CurrentPlayer
    {
        get => currentPlayer;
    }

    public bool IsCPU_Auto = false;

    /// <summary>
    /// 다음 플레이어를 가져오는 프로퍼티(가지고 올 때 currentPlayer도 변경됨)
    /// </summary>
    //public Player NextPlayer
    //{
    //    get
    //    {
    //        //if (currentPlayer.ActionDone)
    //        //{
    //        //    if (currentPlayer.Money >= 0)
    //        //    {
    //        //        playersQueue.Enqueue(currentPlayer);
    //        //    }
    //        //    currentPlayer = playersQueue.Dequeue();

    //        //    if (playersQueue.Count < 1)
    //        //    {
    //        //        // 게임이 끝난 상황
    //        //        Debug.Log($"Game Clear : {currentPlayer.Type} Win");
    //        //    }
    //        //}

    //        return currentPlayer;
    //    }
    //}

    /// <summary>
    /// 초기화 함수(반드시 플레이어가 초기화 된 후에 실행되어야 한다.)
    /// </summary>
    public void Initialize()
    {
        int num = GameManager.Inst.NumOfPlayer - 1;
        playersQueue = new Queue<Player>(num);
        for (int i = 0; i < num; i++)
        {
            playersQueue.Enqueue(GameManager.Inst.Players[i + 1]);
        }

        //테스트 코드
        playersQueue.Clear();
        playersQueue.Enqueue(GameManager.Inst.Players[4]);
        playersQueue.Enqueue(GameManager.Inst.Players[1]);
        playersQueue.Enqueue(GameManager.Inst.Players[2]);
        playersQueue.Enqueue(GameManager.Inst.Players[3]);
    }

    /// <summary>
    /// 최초의 턴을 시작하는 함수
    /// </summary>
    public void GameStart()
    {
        currentPlayer = playersQueue.Dequeue();
        TurnStart();
    }

    Player GetNextPlayer()
    {
        if( currentPlayer.State == PlayerState.TurnEnd )
        {
            if (currentPlayer.Money >= 0)
            {
                currentPlayer.StateChange(PlayerState.WaitStart);   // 대기 상태로 바꾸고
                playersQueue.Enqueue(currentPlayer);    // 플레이어 큐에 추가
            }
            currentPlayer = playersQueue.Dequeue();           

            if (playersQueue.Count < 1)
            {
                // 게임이 끝난 상황
                Debug.Log($"Game Clear : {currentPlayer.Type} Win");
            }
        }

        return currentPlayer;
    }

    void TurnStart()
    {
        // 현재 플레이어 설정
        currentPlayer = GetNextPlayer();

        // 현재 플레이어의 장소별 시작 행동 처리
        Place place = GameManager.Inst.GameMap.GetPlace(currentPlayer.Position);
        PlayerState result = place.TurnStartPlaceAction(currentPlayer);

        

        // 플레이어의 턴 시작
        currentPlayer.StateChange(result);


        //currentPlayer.PlayerTurnStart();
    }

    void TurnEnd()
    {
        //currentPlayer = NextPlayer;
    }

    private void Update()
    {
        if(currentPlayer.State == PlayerState.TurnEnd)
        {
            TurnStart();
        }
    }
}
