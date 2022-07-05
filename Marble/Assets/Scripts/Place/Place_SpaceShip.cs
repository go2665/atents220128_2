using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_SpaceShip : Place
{
    int shipUsePrice = 200;
    bool[] passenger = null;

    private void Start()
    {
        passenger = new bool[GameManager.Inst.NumOfPlayer];
    }

    public override void OnArrive(Player player)
    {
        // UI로 사용 여부 확인
        bool use = false;

        if( use )
        {
            // 탑승할 경우
            CityBase coumbia = GameManager.Inst.GameMap.GetPlace(MapID.Columbia) as CityBase;
            Player ownerPlayer = GameManager.Inst.Players[(int)coumbia.Owner];

            player.Money -= shipUsePrice;
            passenger[(int)player.Type] = true;
            ownerPlayer.Money += shipUsePrice;
        }        
    }

    public override void OnTurnStart(Player player)
    {
        bool user = passenger[(int)player.Type];
        if (user)
        {
            passenger[(int)player.Type] = false;
            // 갈 위치 선택
            MapID target = MapID.Start; // UI를 통해서 변경할 것
            GameManager.Inst.GameMap.Move(player, target);
        }
    }
}