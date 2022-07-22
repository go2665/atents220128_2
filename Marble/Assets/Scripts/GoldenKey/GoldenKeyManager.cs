using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoldenKeyManager : MonoBehaviour
{
    public bool isTestMode = false;
    public GoldenKeyType testCard = GoldenKeyType.ToIsland;

    System.Action<Player>[] keyUseFuncs;
    //System.Action[] shuffleArray;
    Queue<GoldenKeyType> shuffle;
    string[] cardName;
    string[] cardDescription;

    public System.Action<Player> this[GoldenKeyType keyType]
    {
        get => keyUseFuncs[(int)keyType];
    }

    public void Initialize()
    {
        int typeCount = System.Enum.GetValues(typeof(GoldenKeyType)).Length;
        keyUseFuncs = new System.Action<Player>[typeCount];
        keyUseFuncs[(int)GoldenKeyType.ForcedSale] = ForcedSale;
        keyUseFuncs[(int)GoldenKeyType.IncomeTex] = IncomeTex;
        keyUseFuncs[(int)GoldenKeyType.RepairCost] = RepairCost;
        keyUseFuncs[(int)GoldenKeyType.CrimePreventionCost] = CrimePreventionCost;
        keyUseFuncs[(int)GoldenKeyType.IslandEscapeTicket] = IslandEscapeTicket;
        keyUseFuncs[(int)GoldenKeyType.FreePassTicket] = FreePassTicket;
        keyUseFuncs[(int)GoldenKeyType.MoveBack] = MoveBack;
        keyUseFuncs[(int)GoldenKeyType.Trip_Busan] = Trip_Busan;
        keyUseFuncs[(int)GoldenKeyType.Trip_Jeju] = Trip_Jeju;
        keyUseFuncs[(int)GoldenKeyType.Trip_Seoul] = Trip_Seoul;
        keyUseFuncs[(int)GoldenKeyType.ToIsland] = ToIsland;
        keyUseFuncs[(int)GoldenKeyType.GetFund] = GetFund;
        keyUseFuncs[(int)GoldenKeyType.RoundWorld] = RoundWorld;
        keyUseFuncs[(int)GoldenKeyType.MoveSpaceShip] = MoveSpaceShip;
        keyUseFuncs[(int)GoldenKeyType.CruiseTrip] = CruiseTrip;
        keyUseFuncs[(int)GoldenKeyType.AirplaneTrip] = AirplaneTrip;
        keyUseFuncs[(int)GoldenKeyType.Highway] = Highway;
        keyUseFuncs[(int)GoldenKeyType.NobelPrize] = NobelPrize;
        keyUseFuncs[(int)GoldenKeyType.LotteryWin] = LotteryWin;
        keyUseFuncs[(int)GoldenKeyType.RaceWin] = RaceWin;
        keyUseFuncs[(int)GoldenKeyType.ScholarShip] = ScholarShip;
        keyUseFuncs[(int)GoldenKeyType.Pension] = Pension;
        keyUseFuncs[(int)GoldenKeyType.StudyAbroad] = StudyAbroad;
        keyUseFuncs[(int)GoldenKeyType.Hospital] = Hospital;
        keyUseFuncs[(int)GoldenKeyType.Fine] = Fine;
        keyUseFuncs[(int)GoldenKeyType.Birthday] = Birthday;

        cardName = new string[typeCount];
        cardName[(int)GoldenKeyType.ForcedSale] = "반액대매출";
        cardName[(int)GoldenKeyType.IncomeTex] = "종합소득세";
        cardName[(int)GoldenKeyType.RepairCost] = "건물수리비";
        cardName[(int)GoldenKeyType.CrimePreventionCost] = "방범비";
        cardName[(int)GoldenKeyType.IslandEscapeTicket] = "무인도 탈출권";
        cardName[(int)GoldenKeyType.FreePassTicket] = "우대권";
        cardName[(int)GoldenKeyType.MoveBack] = "뒤로 이동";
        cardName[(int)GoldenKeyType.Trip_Busan] = "부산 여행";
        cardName[(int)GoldenKeyType.Trip_Jeju] = "제주도 여행";
        cardName[(int)GoldenKeyType.Trip_Seoul] = "서울 여행";
        cardName[(int)GoldenKeyType.ToIsland] = "조난";
        cardName[(int)GoldenKeyType.GetFund] = "사회복지기금배당";
        cardName[(int)GoldenKeyType.RoundWorld] = "세계일주 초대권";
        cardName[(int)GoldenKeyType.MoveSpaceShip] = "우주여행 초대권";
        cardName[(int)GoldenKeyType.CruiseTrip] = "유람선 여행";
        cardName[(int)GoldenKeyType.AirplaneTrip] = "항공 여행";
        cardName[(int)GoldenKeyType.Highway] = "고속도로";
        cardName[(int)GoldenKeyType.NobelPrize] = "노벨상 수상";
        cardName[(int)GoldenKeyType.LotteryWin] = "복권 당첨";
        cardName[(int)GoldenKeyType.RaceWin] = "자동차 경주 우승";
        cardName[(int)GoldenKeyType.ScholarShip] = "장학금 획득";
        cardName[(int)GoldenKeyType.Pension] = "연금 수령";
        cardName[(int)GoldenKeyType.StudyAbroad] = "해외 유학";
        cardName[(int)GoldenKeyType.Hospital] = "병원비";
        cardName[(int)GoldenKeyType.Fine] = "과속운전 벌금";
        cardName[(int)GoldenKeyType.Birthday] = "생일축하";

        cardDescription = new string[typeCount];
        cardDescription[(int)GoldenKeyType.ForcedSale] = "가장 비싼 땅을 반값에 판매하세요.";
        cardDescription[(int)GoldenKeyType.IncomeTex] = "건물당 정기 종합소득세를 제출하세요.\n\n호텔 150만원.\n빌딩 100만원\n별장 30만원";
        cardDescription[(int)GoldenKeyType.RepairCost] = "건물당 수리비를 제출하세요.\n\n호텔 100만원.\n빌딩 60만원\n별장 30만원";
        cardDescription[(int)GoldenKeyType.CrimePreventionCost] = "건물당 방범비를 제출하세요.\n호텔 50만원.\n빌딩 30만원\n별장 10만원";
        cardDescription[(int)GoldenKeyType.IslandEscapeTicket] = "1회 무인도를 탈출할 수 있습니다.";
        cardDescription[(int)GoldenKeyType.FreePassTicket] = "1회 사용료를 내지 않습니다.";
        cardDescription[(int)GoldenKeyType.MoveBack] = "뒤로 3칸 이동";
        cardDescription[(int)GoldenKeyType.Trip_Busan] = "부산으로 관광을 떠납시다.";
        cardDescription[(int)GoldenKeyType.Trip_Jeju] = "제주도로 관광을 떠납시다.";
        cardDescription[(int)GoldenKeyType.Trip_Seoul] = "서울로 관광을 떠납시다.";
        cardDescription[(int)GoldenKeyType.ToIsland] = "무인도로 이동하세요. 시작점을 지나도 월급을 받을 수 없습니다.";
        cardDescription[(int)GoldenKeyType.GetFund] = "사회복지기금으로 이동해 배당을 받으세요.";
        cardDescription[(int)GoldenKeyType.RoundWorld] = "세계를 한바퀴 돕니다.";
        cardDescription[(int)GoldenKeyType.MoveSpaceShip] = "우주선에 탑승하세요.";
        cardDescription[(int)GoldenKeyType.CruiseTrip] = "퀸 엘리자베스호를 타고 베이징으로 여행을 떠납시다.";
        cardDescription[(int)GoldenKeyType.AirplaneTrip] = "콩코드 여객기를 타고 타이페이로 여행을 떠납시다.";
        cardDescription[(int)GoldenKeyType.Highway] = "출발지로 이동하세요.";
        cardDescription[(int)GoldenKeyType.NobelPrize] = "노벨상을 수상하였습니다. 상금 300만원을 획득했습니다.";
        cardDescription[(int)GoldenKeyType.LotteryWin] = "복권에 당첨되었습니다. 200만원을 얻었습니다.";
        cardDescription[(int)GoldenKeyType.RaceWin] = "자동차 경주에서 우승했습니다. 100만원을 획득했습니다.";
        cardDescription[(int)GoldenKeyType.ScholarShip] = "장학금 100만원을 획득하였습니다.";
        cardDescription[(int)GoldenKeyType.Pension] = "연금 50만원을 수령하였습니다.";
        cardDescription[(int)GoldenKeyType.StudyAbroad] = "해외 유학으로 100만원을 사용했습니다.";
        cardDescription[(int)GoldenKeyType.Hospital] = "병원비로 50만원을 사용했습니다.";
        cardDescription[(int)GoldenKeyType.Fine] = "과속운전으로 벌금이 50만원 나왔습니다.";
        cardDescription[(int)GoldenKeyType.Birthday] = "다른 플레이어들에게 100만원씩 받습니다.";

        // GoldenKeyType을 하나씩 넣은 다음 랜덤하게 섞어 shuffle을 채우기.
        shuffle = Shuffle();
        //shuffleArray = Shuffle(keyUseFuncs);
    }

    public string GetCardName(GoldenKeyType type) => cardName[(int)type];    

    public string GetCardDescription(GoldenKeyType type) => cardDescription[(int)type];    

    public GoldenKeyType DrawCard()
    {
        if( shuffle.Count <= 0 )
        {
            shuffle = Shuffle();
        }

        GoldenKeyType card = shuffle.Dequeue();
        return card;
    }

    public void RunGoldenCard(GoldenKeyType type, Player player)
    {
        this[type]?.Invoke(player);
    }

    System.Action[] Shuffle(System.Action[] original)
    {
        int typeCount = System.Enum.GetValues(typeof(GoldenKeyType)).Length;
        System.Action[] result = new System.Action[typeCount];
        for(int i=0; i<typeCount;i++)
        {
            result[i] = original[i];
        }

        for(int i=typeCount-1; i>0;i--)
        {
            int index = Random.Range(0, i);
            System.Action temp = result[i];
            result[i] = result[index];
            result[index] = temp;
        }

        return result;
    }

    Queue<GoldenKeyType> Shuffle()
    {
        int typeCount = System.Enum.GetValues(typeof(GoldenKeyType)).Length;
        List<GoldenKeyType> temp = new List<GoldenKeyType>(typeCount);
        for (int i = 0; i < typeCount; i++)
        {
            temp.Add((GoldenKeyType)i);
        }
        Queue<GoldenKeyType> result = new Queue<GoldenKeyType>(typeCount);
        while(temp.Count>0)
        {
            int index = Random.Range(0, temp.Count);
            GoldenKeyType randomType = temp[index];
            temp.RemoveAt(index);
            result.Enqueue(randomType);
        }

        if( isTestMode )
        {
            result.Clear();
            for (int i = 0; i < typeCount; i++)
            {
                result.Enqueue(testCard);
            }
        }

        return result;
    }

    void ForcedSale(Player player) 
    {
        Debug.Log("ForcedSale");
        CityBase target = player.FindHighestTotalvalue();
        if (target != null)
        {
            target.Sell(PlayerType.Bank);
        }
    }

    void IncomeTex(Player player) 
    {
        Debug.Log("IncomeTex");
        player.PayMaintenenceCost(30,100,150);
    }
    void RepairCost(Player player)
    {
        Debug.Log("RepairCost");
        player.PayMaintenenceCost(30, 60, 100);
    }
    void CrimePreventionCost(Player player)
    {
        Debug.Log("CrimePreventionCost");
        player.PayMaintenenceCost(10, 30, 50);
    }
    void IslandEscapeTicket(Player player)
    {
        Debug.Log("IslandEscapeTicket");
        player.AddGoldenKey(GoldenKeyType.IslandEscapeTicket);
    }
    void FreePassTicket(Player player)
    {
        Debug.Log("FreePassTicket");
        player.AddGoldenKey(GoldenKeyType.FreePassTicket);
    }
    void MoveBack(Player player)
    {
        Debug.Log("MoveBack");

        int newPosition = (int)player.Position - 3;
        if(newPosition < 0)
        {
            newPosition += 40;
        }

        GameManager.Inst.GameMap.SetPosition(player, (MapID)newPosition);
    }
    void Trip_Busan(Player player)
    {
        Debug.Log("Trip_Busan");
        player.Move(MapID.Busan);
    }
    void Trip_Jeju(Player player)
    {
        Debug.Log("Trip_Jeju");
        player.Move(MapID.Jeju);
    }
    void Trip_Seoul(Player player)
    {
        Debug.Log("Trip_Seoul");
        player.Move(MapID.Seoul);
    }
    void ToIsland(Player player)
    {
        Debug.Log("ToIsland");
        GameManager.Inst.GameMap.SetPosition(player, MapID.Island);
    }
    void GetFund(Player player)
    {
        Debug.Log("GetFund");
        player.Move(MapID.Fund_Get);
    }
    void RoundWorld(Player player)
    {
        Debug.Log("RoundWorld");
        Place_FundGet fundGet = GameManager.Inst.GameMap.GetPlace(MapID.Fund_Get) as Place_FundGet;
        fundGet.GetFund(player);
        player.Move(40);
    }
    void MoveSpaceShip(Player player)
    {
        Debug.Log("MoveSpaceShip");
        player.Move(MapID.SpaceShip);
    }
    void CruiseTrip(Player player)
    {
        Debug.Log("CruieTrip");
        CityBase ship = GameManager.Inst.GameMap.GetPlace(MapID.QueenElizabeth) as CityBase;
        ship.PayUsePrice(player);
        player.Move(MapID.Beijing);
    }
    void AirplaneTrip(Player player)
    {
        Debug.Log("AirplaneTrip");
        CityBase air = GameManager.Inst.GameMap.GetPlace(MapID.Concord) as CityBase;
        air.PayUsePrice(player);
        player.Move(MapID.Taipei);
    }
    void Highway(Player player)
    {
        Debug.Log("Highway");
        player.Move(MapID.Start);
    }
    void NobelPrize(Player player)
    {
        Debug.Log("NobelPrize");
        player.Money += 300;
    }
    void LotteryWin(Player player)
    {
        Debug.Log("LotteryWin");
        player.Money += 200;
    }
    void RaceWin(Player player)
    {
        Debug.Log("RaceWin");
        player.Money += 100;
    }
    void ScholarShip(Player player)
    {
        Debug.Log("SchilarShip");
        player.Money += 100;
    }
    void Pension(Player player)
    {
        Debug.Log("Pension");
        player.Money += 50;
    }
    void StudyAbroad(Player player)
    {
        Debug.Log("StudyAbroad");
        player.Money -= 100;
    }
    void Hospital(Player player)
    {
        Debug.Log("Hospital");
        player.Money -= 50;
    }
    void Fine(Player player)
    {
        Debug.Log("Fine");
        player.Money -= 50;
    }
    void Birthday(Player player)
    {
        Debug.Log("Birthday");
        int present = 0;
        int baseMoney = 100;
        foreach(var other in GameManager.Inst.Players)
        {
            if (other != player && other.Type != PlayerType.Bank)
            {
                if( other.Money < baseMoney)
                {
                    int money = Mathf.Clamp(other.Money, 0, other.Money);

                    other.Money -= money;
                    present += money;
                }
                else
                {
                    other.Money -= baseMoney;
                    present += baseMoney;
                }
            }
        }
        player.Money += present;
    }
}
