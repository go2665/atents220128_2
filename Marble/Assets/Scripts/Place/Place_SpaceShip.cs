using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_SpaceShip : Place
{
    public const int shipUsePrice = 200;
    //bool[] passenger = null;

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.SpaceShip);
        cover.transform.Rotate(0, 0, 180);
    }

    private void Start()
    {
        //passenger = new bool[GameManager.Inst.NumOfPlayer];
    }

    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : 우주왕복선입니다.");
        // UI로 사용 여부 확인

        GameManager.Inst.UI_Manager.ShowSpaceShipPanel(true, player);

        //bool use = false;

        //if( use )
        //{
        //    // 탑승할 경우
        //    CityBase coumbia = GameManager.Inst.GameMap.GetPlace(MapID.Columbia) as CityBase;
        //    Player ownerPlayer = GameManager.Inst.Players[(int)coumbia.Owner];

        //    player.Money -= shipUsePrice;
        //    passenger[(int)player.Type] = true;
        //    ownerPlayer.Money += shipUsePrice;
        //}
        base.OnArrive(player);
    }

    //public override void OnTurnStart(Player player)
    //{
    //    bool user = passenger[(int)player.Type];
    //    if (user)
    //    {
    //        passenger[(int)player.Type] = false;
    //        // 갈 위치 선택
    //        MapID target = MapID.Start; // UI를 통해서 변경할 것
    //        GameManager.Inst.GameMap.Move(player, target);
    //    }
    //}
}
