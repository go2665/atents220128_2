using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Island : Place
{
    int waitCount = 3;

    public override void OnArrive(Player player)
    {
        player.OnArriveIsland(waitCount);
    }

    public override void OnTurnStart(Player player)
    {
        // 주사위 굴려서 더블 나오는지 확인 => 더블 나오면 나온 숫자 그대로 이동
    }
}
