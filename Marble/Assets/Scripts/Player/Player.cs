using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어(인간, 컴퓨터, 은행 포함)
/// </summary>
public class Player : MonoBehaviour
{
    DiceSet dice;
    Map map;    

    const int StartMoney = 5000;        // 시작 금액

    PlayerType type = PlayerType.Bank;  // 플레이어 타입 설정

    int money;                          // 보유 금액    
    MapID position = MapID.Start;       // Start 위치로 초기 위치 설정
    
    Material material;                  // 플레이어의 색상을 지정하기 위해 사용


    bool panelWaiting = false;
    int actionCount = 0;                // 행동력
    int islandWaitTime = -1;            // 무인도 대기 시간

    readonly List<CityBase> ownedCities = new();    // 보유한 도시 목록
    readonly List<GoldenKeyType> ownedGoldenKey = new();

    /// <summary>
    /// 보유 금액 프로퍼티
    /// </summary>
    public int Money 
    { 
        get => money; 
        set
        {
            if (money != value)
            {
                money = value;
                if (money < 0)
                {
                    GameManager.Inst.UI_Manager.ShowBankruptcyPanel(true, this);
                }
                OnMoneyChange?.Invoke(this);   // 금액에 변동이 있으면 OnMoneyChange 델리게이트 실행
            }
        }
    }

    /// <summary>
    /// 플레이어의 위치용 프로퍼티
    /// </summary>
    public MapID Position
    {
        get => position;
        set
        {
            position = value;
        }
    }

    /// <summary>
    /// 플레이어의 종류 프로퍼티(읽기전용)
    /// </summary>
    public PlayerType Type
    {
        get => type;
    }

    /// <summary>
    /// 행동력 다 사용했는지 표시하는 프로퍼티(읽기전용)
    /// </summary>
    public bool ActionDone
    {
        get => actionCount < 1;
    }

    /// <summary>
    /// 돈에 변화가 있을 때 실행될 델리게이트
    /// </summary>
    public System.Action<Player> OnMoneyChange;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        dice = GetComponent<DiceSet>();
        dice.OnDouble += OnDouble;
    }

    /// <summary>
    /// 초기화 함수(반드시 주사위와 맵이 초기화 된 이후에 실행되어야 함)
    /// </summary>
    /// <param name="playerType">이 플레이어가 초기화 될 타입</param>
    public void Initialize(PlayerType playerType)
    {
        type = playerType;
        material.color = GameManager.Inst.PlayerColor[(int)type];  // 플레이어의 색상 설정
        if (type == PlayerType.Bank)
        {
            // 은행일 경우
            Money = 2000000000;
            gameObject.SetActive(false);
        }
        else
        {
            Money = StartMoney;  
        }

        gameObject.name = $"Player_{playerType}";   // 플레이어 이름 설정

        map = GameManager.Inst.GameMap;
    }

    /// <summary>
    /// 무인도 도착했을 때 실행될 함수
    /// </summary>
    /// <param name="wait">기다리는 시간(기본3)</param>
    public void OnArriveIsland(int wait)
    {
        islandWaitTime = wait;
    }

    public void PlayerTurnStart()
    {
        actionCount = 1;    // 행동력 1로 회복
        PlayerPlaceActionProcess();
    }

    public void PlayerPlaceActionProcess()
    {
        Place place = map.GetPlace(Position);
        place.TurnStartPlaceAction(this);
        PlayerMoveProcess();
    }

    public void PlayerMoveProcess()
    {
        if (islandWaitTime < 0) // 무인도에서 대기하는 상황이 아닐 때만 이동
        {
            if (Type != PlayerType.Human)
            {
                // CPU 플레이어들            
                MoveRollDice();
            }
            else
            {
                // 인간 플레이어
                OnPanelOpen();
                GameManager.Inst.UI_Manager.ShowDiceRollPanel(true);    // 패널 열어서 주사위 굴리기
            }
        }

        if(!panelWaiting)
        {
            PlayerTurnEnd();
        }
    }

    public void PlayerTurnEnd()
    {
        // 각 플레이스의 OnArrive 끝날 때 자동 실행
        if (ActionDone)
        {
            //StartCoroutine(TurnEndWait());
            Player player = GameManager.Inst.TurnManager.NextPlayer;
            player.PlayerTurnStart();
        }
        else
        {
            PlayerMoveProcess();
        }
    }

    IEnumerator TurnEndWait()
    {
        if (GameManager.Inst.TurnManager != null)
        {
            Player player = GameManager.Inst.TurnManager.NextPlayer;
            yield return new WaitForSeconds(0.3f);
            player.PlayerTurnStart();
        }
    }

    /// <summary>
    /// 더블이 나왔을 때 실행될 함수
    /// </summary>
    /// <param name="diceThrower"></param>
    void OnDouble()
    {
        //if (diceThrower == PlayerType.Human)
        //{
        Debug.Log($"{Type}이 더블이 나왔습니다.");
        //}

        if (position == MapID.Island)
        {
            islandWaitTime = 0; // 무인도에 있는 상황이면 무인도 탈출 조건 성립 시킴
        }
            
        actionCount++;          // 한번 더 던지도록 행동 횟수 추가
    }

    /// <summary>
    /// 주사위를 굴리는 함수
    /// </summary>
    public void MoveRollDice()
    {
        // 행동력이 있을 때만 실행
        if (actionCount > 0)
        {
            actionCount--;  // 행동력 감소

            //// 주사위 돌리는 애니메이션 등 처리            
            string str = $"{Type}은 {this.Position}에서 ";
            
            // 주사위 결과에 따라 이동처리
            int dicesum = dice.RollAll_GetTotalSum(Type == PlayerType.Human);   // 주사위 굴리고
            Move(dicesum);    // 이동시키기
            str += $"{this.Position}에 도착했습니다.";
            Debug.Log($"{Type}은 {dicesum}이 나왔습니다. 더블[{dice.IsLastDouble}]");
            Debug.Log(str);
            GameManager.Inst.UI_Manager.SetResultText(Type, dicesum);   // 결과를 결과창에 띄우기
        }
    }

    public void BuyCity(CityBase city)
    {
        ownedCities.Add(city);
    }

    public void SellCity(CityBase city)
    {
        ownedCities.Remove(city);
    }

    public City FindNoBuildCity()
    {
        City target = null;
        float efficient = 0;
        foreach( CityBase cityBase in ownedCities )
        {
            City city = cityBase as City;
            if (city != null)
            {
                for (int i = 2; i >= 0; i--)
                {                    
                    float temp = city.buildingDatas[i].usePrice / city.buildingDatas[i].price;
                    if(temp > efficient)
                    {
                        efficient = temp;
                        target = city;
                    }
                }
            }
        }
        return target;
    }

    public CityBase FindHighestTotalvalue()
    {
        int highestValue = 0;
        CityBase highestCity = null;
        foreach(var city in ownedCities)
        {
            if( highestValue < city.TotalValue )
            {
                highestValue = city.TotalValue;
                highestCity = city;
            }
        }

        return highestCity;
    }

    public void PayMaintenenceCost(int VilaCost, int BuildingCost, int hotelCost)
    {
        int totalCost = 0;
        foreach(CityBase cityBase in ownedCities)
        {
            City city = cityBase as City;
            if( city != null )
            {
                totalCost += city.buildingDatas[(int)BuildingType.Villa].count * VilaCost;
                totalCost += city.buildingDatas[(int)BuildingType.Building].count * BuildingCost;
                totalCost += city.buildingDatas[(int)BuildingType.Hotel].count * hotelCost;
            }
        }

        Money -= totalCost;
    }

    public void AddGoldenKey(GoldenKeyType type)
    {
        ownedGoldenKey.Add(type);
    }

    public void UseGoldenKey(GoldenKeyType type)
    {
        if(type == GoldenKeyType.IslandEscapeTicket)
        {
            islandWaitTime = 0;
        }
        else
        {

        }
        ownedGoldenKey.Remove(type);
    }

    public bool HaveGoldenKey(GoldenKeyType type)
    {
        return ownedGoldenKey.Exists((x) => x == type);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="order">true면 오름차순, false면 내림차순</param>
    /// <returns></returns>
    public List<CityBase> OnBankrupt(bool order)
    {
        List<CityBase> list = new List<CityBase>(ownedCities.Count);

        if (order)
        {
            ownedCities.Sort((city1, city2) => city1.TotalValue.CompareTo(city2.TotalValue));   //오름차순
        }
        else
        {
            ownedCities.Sort((city1, city2) => city2.TotalValue.CompareTo(city1.TotalValue));   //내림차순
        }

        int totalSellPrice = 0;
        foreach(var city in ownedCities)
        {
            list.Add(city);
            totalSellPrice += (city.TotalValue >> 1);
            if(totalSellPrice + Money >= 0 )
            {
                break;
            }
        }

        return list;
    }

    public void TestDice(int one, int two)
    {
        dice.isTest = true;
        dice.testDice1 = one;
        dice.testDice2 = two;
    }
    public bool TryDiceDouble() => dice.TryDouble();

    public void SetStartPosition(MapID mapID)
    {
        Place place = map.GetPlace(mapID);
        transform.position = place.GetPlayerPosition(Type);
    }

    /// <summary>
    /// 맵에 있는 플레이어를 이동시키는 함수
    /// </summary>
    /// <param name="count">이동시킬 칸 수</param>
    public void Move(int count)
    {
        int dest = (int)Position + count;    // 목적지 계산하기
        if (dest >= Map.NumOfPlaces)         // 한바퀴 돌았을 경우
        {
            dest -= Map.NumOfPlaces;
            Place_Start start = map.GetPlace(MapID.Start) as Place_Start;
            start.GetSalaray(this);   // 월급 추가
        }

        Position = (MapID)dest; // 실제 위치 변경
        Place place = map.GetPlace(Position);
        transform.position = place.GetPlayerPosition(Type);   // 말 위치 변경
                
        place.OnArrive(this);         // 장소 도착 함수 실행
    }

    /// <summary>
    /// 맵에 있는 플레이어를 이동시키는 함수(특정 칸으로 즉시 이동할 때 사용할 함수)
    /// </summary>
    /// <param name="mapID">이동할 장소</param>
    public void Move(MapID mapID)
    {
        int move = mapID - Position;
        if (move < 0)
        {
            move = Map.NumOfPlaces + move;
        }
        Move(move);
    }

    public int IslandWait()
    {
        islandWaitTime--;
        return islandWaitTime;
    }

    public void OnPanelOpen()
    {
        panelWaiting = true;
    }

    public void OnPanelClose()
    {
        panelWaiting = false;
    }
}
