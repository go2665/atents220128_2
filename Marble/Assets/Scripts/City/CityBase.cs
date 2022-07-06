using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBase : Place
{
    public int price;      // 구매 비용
    public int usePrice;   // 다른 플레이어가 들어왔을때 지불할 비용

    protected PlayerType owner = PlayerType.Bank;
    protected int value;

    public PlayerType Owner
    {
        get => owner;
    }

    public int Value
    {
        get
        {
            return value;
        }
    }

    public bool CanBuy(PlayerType buyer)
    {
        return (GameManager.Inst.Players[(int)buyer].Money > price);
    }

    public void Sell(PlayerType buyer, int sellPrice)
    {
        // 이 도시 사고 팔기
        Player ownerPlayer = GameManager.Inst.Players[(int)owner];
        Player buyerPlayer = GameManager.Inst.Players[(int)buyer];

        ownerPlayer.Money += sellPrice;
        buyerPlayer.Money -= sellPrice;

        owner = buyer;
    }

    protected virtual void Start()
    {
        value = price;
    }

    public override void OnArrive(Player player)
    {
        if (owner == PlayerType.Bank)
        {
            // 은행 땅이다. => 구매 여부 확인
        }
        else if (owner != player.Type)
        {
            // 남의 땅이다.
            player.Money -= usePrice;
        }
    }

    public override void Initialize(GameObject obj, ref MapData mapData)
    {
        base.Initialize(obj, ref mapData);
        price = mapData.placeBuyPrice;
        usePrice = mapData.placeUsePrice;
    }
}
