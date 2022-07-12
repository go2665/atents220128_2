using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무인도
/// </summary>
public class Place_Island : Place
{
    int waitCount = 4;  // 3턴 쉬게 된다. 4턴째에는 바로 출발

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Island);
    }

    public override void OnArrive(Player player)
    {
        player.OnArriveIsland(waitCount);
        //Debug.Log($"{player} : 무인도에 도착했습니다.");
        base.OnArrive(player);
    }
}
