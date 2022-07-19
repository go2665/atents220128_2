using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBase : Place
{
    public GameObject playerColorLayerPrefab;
    protected GameObject playerColorLayer;
    Renderer playerColorLayerRenderer;

    public int price;       // 구매 비용
    public int usePrice;    // 다른 플레이어가 들어왔을때 지불할 비용

    protected PlayerType owner = PlayerType.Bank;
    protected int totalValue;       // 구매가격의 합(citybase는 땅가격만. city는 building 구매비용 추가)
    protected int totalUsePrice;    // 사용 요금의 합(citybase는 땅 사용료만. city는 building 사용료 추가)

    const int UseDefaultPrice = -1;

    public PlayerType Owner
    {
        get => owner;
    }

    public int TotalValue
    {
        get => totalValue;        
    }

    public int TotalUsePrice
    {
        get => totalUsePrice;
    }

    protected virtual void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.None);
    }

    protected virtual void Start()
    {
        totalValue = price;
        totalUsePrice = usePrice;
    }

    /// <summary>
    /// 구매 가능여부 확인
    /// </summary>
    /// <param name="buyer">확인할 플레이어</param>
    /// <returns>구매 가능하면 true</returns>
    public bool CanBuy(PlayerType buyer)
    {
        return (GameManager.Inst.Players[(int)buyer].Money > price);
    }

    /// <summary>
    /// 도시를 판매하는 함수
    /// </summary>
    /// <param name="buyer">구매자</param>
    /// <param name="sellPrice">판매가격. 디폴트 값일 경우 기본 구매가격 사용</param>
    public virtual void Sell(PlayerType buyer, int sellPrice = UseDefaultPrice)
    {
        if(sellPrice == UseDefaultPrice)
        {
            sellPrice = price;
        }
        if( buyer == PlayerType.Bank )  // 은행에 팔 때는 무조건 반값
        {
            sellPrice >>= 1;
        }

        // 이 도시 사고 팔기
        Player ownerPlayer = GameManager.Inst.GetPlayer(owner);
        Player buyerPlayer = GameManager.Inst.GetPlayer(buyer);

        ownerPlayer.Money += sellPrice;
        ownerPlayer.SellCity(this);
        buyerPlayer.Money -= sellPrice;
        buyerPlayer.BuyCity(this);

        owner = buyer;
        if (buyer == PlayerType.Bank)
        {
            // 강제 매각(반액대매출 또는 부도 전에 지불하기)
            playerColorLayer.SetActive(false);
        }
        else
        {
            //오너의 색깔 입히기
            playerColorLayerRenderer.material.color = GameManager.Inst.PlayerColor[(int)owner];
            playerColorLayer.SetActive(true);
        }
    }

    /// <summary>
    /// 이 도시에 누군가 도착했을 때 실행될 함수
    /// </summary>
    /// <param name="player">도착한 사람</param>
    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : {placeName}에 도착했습니다.");
        if (owner == PlayerType.Bank)
        {
            // 은행 땅이다. => 구매 여부 확인
            if (player.Type == PlayerType.Human)
            {
                // 사람 플레이어일 경우 UI 띄워서 확인
                GameManager.Inst.UI_Manager.ShowBuyPanel(true, player, this);
            }
            else
            {
                // CPU 플레이어일 경우 살 수 있으면 무조건 구매
                if( CanBuy(player.Type) )
                {
                    Sell(player.Type);
                }
                base.OnArrive(player);  // 턴 넘기기
            }
        }
        else if (owner != player.Type)
        {
            // 남의 땅이다.
            if (player.HaveGoldenKey(GoldenKeyType.FreePassTicket))
            {
                if (player.Type == PlayerType.Human)
                {
                    GameManager.Inst.UI_Manager.ShowUseGoldenKeyPanel(true, player, GoldenKeyType.FreePassTicket);
                }
                else
                {
                    player.UseGoldenKey(GoldenKeyType.FreePassTicket);
                    player.PlayerTurnEnd();
                }
            }
            else
            {
                PayUsePrice(player);
                base.OnArrive(player);
            }
        }
    }

    public void PayUsePrice(Player target)
    {
        target.Money -= totalUsePrice;       // 돈 지불하기
        Player ownerPlayer = GameManager.Inst.GetPlayer(owner);
        ownerPlayer.Money += totalUsePrice;  // 소유주의 금액 증가
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
