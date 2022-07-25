using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무인도
/// </summary>
public class Place_Island : Place
{
    int waitCount = 3;  // 3턴 쉬게 된다. 4턴째에는 바로 출발

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Island);
    }

    public override void OnArrive(Player player)
    {        
        //Debug.Log($"{player} : 무인도에 도착했습니다.");
        base.OnArrive(player);
    }

    protected override void ArrivePlaceAction(Player player)
    {
        Debug.Log($"{player} : 무인도에서 {waitCount}턴간 대기.");
        player.OnArriveIsland(waitCount);
        base.ArrivePlaceAction(player);
    }

    public override void TurnStartPlaceAction(Player player)
    {
        int remindTurn = player.IslandWait();
        Debug.Log($"{player} : 무인도 {remindTurn}턴 남음.");

        if (remindTurn > -1)
        {
            // 대기시간이 남아있다.
            // 무인도 탈출권이 있는지 확인.
            if (player.HaveGoldenKey(GoldenKeyType.IslandEscapeTicket))
            {
                Debug.Log("무인도 탈출권 있음");
                if (player.Type == PlayerType.Human)
                {
                    // 사람이면 사용할지 물어보기
                    //GameManager.Inst.UI_Manager.ShowUseGoldenKeyPanel(true, player, GoldenKeyType.IslandEscapeTicket);
                    //return;
                }
                else
                {
                    // CPU면 그냥 사용하기
                    player.UseGoldenKey(GoldenKeyType.IslandEscapeTicket);
                }
            }

            // 무인도 탈출권이 있는데
            // 1. 사용했다.     -> 주사위 굴릴 필요가 없음
            // 2. 사용안했다.   -> 주사위를 굴려야 함

            //탈출권이 없으면 더블로 탈출 시도
            if (player.Type == PlayerType.Human)
            {
                player.OnPanelOpen();
                GameManager.Inst.UI_Manager.ShowIslandEscapePanel(true);
            }
            else
            {
                if (player.TryEscapeIsland())
                {
                    player.OnArriveIsland(0);
                }
            }                
        }        
    }
}
