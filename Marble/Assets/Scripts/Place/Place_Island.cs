using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Island : Place
{
    int waitCount = 3;

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Island);
    }

    public override void OnArrive(Player player)
    {
        player.OnArriveIsland(waitCount);
        Debug.Log($"{player} : 무인도에 도착했습니다.");
    }

    public override void OnTurnStart(Player player)
    {
        // 주사위 굴려서 더블 나오는지 확인 => 더블 나오면 나온 숫자 그대로 이동
    }
}
