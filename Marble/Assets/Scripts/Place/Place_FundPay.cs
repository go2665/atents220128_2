using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기금에 돈을 지불하는 장소
/// </summary>
public class Place_FundPay : Place
{
    int fundPrice = 150;
    Place_FundGet fundGet;

    void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.FundPay);
    }

    private void Start()
    {
        fundGet = GameManager.Inst.GameMap.GetPlace(MapID.Fund_Get) as Place_FundGet;
    }

    /// <summary>
    /// 플레이어가 도착했을 때 실행되는 함수. 기금에 돈을 지급
    /// </summary>
    /// <param name="player">도착한 플레이어</param>
    public override void OnArrive(Player player)
    {
        player.Money -= fundPrice;
        
        if( fundGet != null )
        {
            fundGet.AddFund(fundPrice);
        }
        //Debug.Log($"{player} : 기금에 {fundPrice}원을 지불합니다.");
        base.OnArrive(player);
    }
}
