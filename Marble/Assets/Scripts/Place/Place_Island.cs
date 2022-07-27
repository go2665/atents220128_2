using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무인도
/// </summary>
public class Place_Island : Place
{
    int waitTime = 3;  // 3턴 쉬게 된다. 4턴째에는 바로 출발
    
    class Victim
    {
        public Player player;
        public int waitRemain;
        public Victim(Player p, int waitTime)
        {
            player = p;
            waitRemain = waitTime;
        }
    }
    List<Victim> victimList;    // 무인도에 갇힌 사람의 목록

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Island);
        victimList = new List<Victim>(4);
    }

    //public override void OnArrive(Player player)
    //{        
    //    //Debug.Log($"{player} : 무인도에 도착했습니다.");
    //    base.OnArrive(player);
    //}

    PlayerState TryIslandEscape(Player player)
    {
        PlayerState result = PlayerState.TurnEnd;
        if (player.TryIslandEscapeRoll())
        {
            Debug.Log($"{player} : 더블. 무인도에서 탈출 성공.");
            GameManager.Inst.UI_Manager.ShowMessagePanel(true, player, "무인도에서 탈출 성공!");
            EscapeIsland(player);
            result = PlayerState.DiceRoll;
        }
        else
        {
            GameManager.Inst.UI_Manager.ShowMessagePanel(true, player, "무인도에서 탈출 실패...");
            Debug.Log($"{player} : 무인도에서 탈출 시도 실패");
        }

        return result;
    }


    protected override void ArrivePlaceAction(Player player)
    {
        Debug.Log($"{player} : 무인도에서 {waitTime}턴간 대기.");
        //player.OnArriveIsland(waitCount);
        victimList.Add(new(player, waitTime));

        if (player.IsLastDiceDouble())
        {
            PlayerState result = TryIslandEscape(player);
            player.StateChange(result);
        }

        base.ArrivePlaceAction(player);
    }

    public override PlayerState TurnStartPlaceAction(Player player)
    {
        PlayerState playerState = PlayerState.DiceRoll;

        //victimList에 플레이어가 있으면 갇힌상태
        Victim victim = victimList.Find((x) => x.player == player);
        if (victim != null)
        {
            // 갇힌 사람만 처리하자
                        
            // 무인도 탈출권이 있는지 확인.
            if (player.HaveGoldenKey(GoldenKeyType.IslandEscapeTicket))
            {
                Debug.Log("무인도 탈출권 있음");
                if (player.Type == PlayerType.Human)
                {
                    // 사람이면 사용할지 물어보기
                    GameManager.Inst.UI_Manager.ShowUseGoldenKeyPanel(true, player, GoldenKeyType.IslandEscapeTicket);
                    return PlayerState.WaitPanelResponse;

                    // Player의 상태 중에 대기 상태 만들기
                    // 대기 상태로 들어가면 아무것도 안함
                    // 대기 상태 종료 함수가 호출되면 미리 지정한 상태(대기 상태로 들어갈 때 기록해 놓기)로 전환
                }
                else
                {
                    // CPU면 그냥 사용하기
                    player.UseGoldenKey(GoldenKeyType.IslandEscapeTicket);
                    GameManager.Inst.UI_Manager.ShowMessagePanel(true, player, "무인도 탈출권 사용!");                    
                    return playerState;
                }
            }

            // 더블이 나오는가?
            playerState = TryIslandEscape(player);
            if(playerState == PlayerState.DiceRoll)
            {
                return playerState;
            }
            //if(player.TryIslandEscapeRoll())
            //{
            //    Debug.Log($"{player} : 더블. 무인도에서 탈출 성공.");
            //    GameManager.Inst.UI_Manager.ShowMessagePanel(true, player, "무인도에서 탈출 성공!" );
            //    EscapeIsland(player);
            //    return playerState;
            //}
            //else
            //{
            //    GameManager.Inst.UI_Manager.ShowMessagePanel(true, player, "무인도에서 탈출 실패...");
            //    Debug.Log($"{player} : 무인도에서 탈출 시도 실패");
            //}

            victim.waitRemain--;
            if (victim.waitRemain < 1)
            {
                Debug.Log($"{player} : 무인도에서 나갈 시간.");
                EscapeIsland(player);   // 코드 통일성 유지를 위해
                return playerState;
            }

            Debug.Log($"{player} : 무인도 {victim.waitRemain}턴 남음.");
            playerState = PlayerState.TurnEnd;
        }

        return playerState;

        //int remindTurn = player.IslandWait();
        //Debug.Log($"{player} : 무인도 {remindTurn}턴 남음.");

        //if (remindTurn > -1)
        //{
        //    // 대기시간이 남아있다.
        //    // 무인도 탈출권이 있는지 확인.
        //    if (player.HaveGoldenKey(GoldenKeyType.IslandEscapeTicket))
        //    {
        //        Debug.Log("무인도 탈출권 있음");
        //        if (player.Type == PlayerType.Human)
        //        {
        //            // 사람이면 사용할지 물어보기
        //            //GameManager.Inst.UI_Manager.ShowUseGoldenKeyPanel(true, player, GoldenKeyType.IslandEscapeTicket);
        //            //return;
        //        }
        //        else
        //        {
        //            // CPU면 그냥 사용하기
        //            player.UseGoldenKey(GoldenKeyType.IslandEscapeTicket);
        //        }
        //    }

        //    // 무인도 탈출권이 있는데
        //    // 1. 사용했다.     -> 주사위 굴릴 필요가 없음
        //    // 2. 사용안했다.   -> 주사위를 굴려야 함

        //    //탈출권이 없으면 더블로 탈출 시도
        //    if (player.Type == PlayerType.Human)
        //    {
        //        player.OnPanelOpen();
        //        GameManager.Inst.UI_Manager.ShowIslandEscapePanel(true);
        //    }
        //    else
        //    {
        //        if (player.TryEscapeIsland())
        //        {
        //            //player.OnArriveIsland(0);
        //        }
        //    }                
        //}        
    }

    public void EscapeIsland(Player player)
    {
        Victim victim = victimList.Find((x) => x.player == player);
        if (victim != null)
        {
            victimList.Remove(victim);
        }
    }

    /// <summary>
    /// 플레이어가 무인도에 갇혔는지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="player">확인할 사람</param>
    /// <returns>true면 갇힌 사람</returns>
    public bool IsVictim(Player player)
    {
        return (victimList.Find((x) => x.player == player) != null);
    }
}
