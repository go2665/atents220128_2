using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBase : Place
{
    public GameObject playerColorLayerPrefab;
    protected GameObject playerColorLayer;
    Renderer playerColorLayerRenderer;

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

    protected virtual void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.None);
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

        playerColorLayerRenderer.material.color = GameManager.Inst.PlayerColor[(int)owner];
        playerColorLayer.SetActive(true);
    }

    protected virtual void Start()
    {
        value = price;
    }

    public override void OnArrive(Player player)
    {
        Debug.Log($"{player} : {placeName}에 도착했습니다.");
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

        playerColorLayer = Instantiate(playerColorLayerPrefab, transform);
        playerColorLayerRenderer = playerColorLayer.GetComponent<Renderer>();
        playerColorLayer.SetActive(false);
    }
}
