using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoldenKeyManager : MonoBehaviour
{    
    System.Action<Player>[] keyUseFuncs;
    //System.Action[] shuffleArray;
    List<GoldenKeyType> shuffle;

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

        // GoldenKeyType을 하나씩 넣은 다음 랜덤하게 섞어 shuffle을 채우기.
        shuffle = Shuffle();
        //shuffleArray = Shuffle(keyUseFuncs);
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

    List<GoldenKeyType> Shuffle()
    {
        int typeCount = System.Enum.GetValues(typeof(GoldenKeyType)).Length;
        List<GoldenKeyType> temp = new List<GoldenKeyType>(typeCount);
        for (int i = 0; i < typeCount; i++)
        {
            temp.Add((GoldenKeyType)i);
        }
        List<GoldenKeyType> result = new List<GoldenKeyType>(typeCount);
        while(temp.Count>0)
        {
            int index = Random.Range(0, temp.Count);
            GoldenKeyType randomType = temp[index];
            result.Add(randomType);
            temp.RemoveAt(index);
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
        GameManager.Inst.GameMap.Move(player, MapID.Busan);
    }
    void Trip_Jeju(Player player)
    {
        Debug.Log("Trip_Jeju");
        GameManager.Inst.GameMap.Move(player, MapID.Jeju);
    }
    void Trip_Seoul(Player player)
    {
        Debug.Log("Trip_Seoul");
        GameManager.Inst.GameMap.Move(player, MapID.Seoul);
    }
    void ToIsland(Player player)
    {
        Debug.Log("ToIsland");
        GameManager.Inst.GameMap.SetPosition(player, MapID.Island);
    }
    void GetFund(Player player)
    {
        Debug.Log("GetFund");
        GameManager.Inst.GameMap.Move(player, MapID.Fund_Get);
    }
    void RoundWorld(Player player)
    {
        Debug.Log("RoundWorld");
        Place_FundGet fundGet = GameManager.Inst.GameMap.GetPlace(MapID.Fund_Get) as Place_FundGet;
        fundGet.GetFund(player);
        GameManager.Inst.GameMap.Move(player, 40);
    }
    void MoveSpaceShip(Player player)
    {
        Debug.Log("MoveSpaceShip");
        GameManager.Inst.GameMap.Move(player, MapID.SpaceShip);
    }
    void CruiseTrip(Player player)
    {
        Debug.Log("CruieTrip");
        CityBase ship = GameManager.Inst.GameMap.GetPlace(MapID.QueenElizabeth) as CityBase;
        ship.PayUsePrice(player);
        GameManager.Inst.GameMap.Move(player, MapID.Beijing);
    }
    void AirplaneTrip(Player player)
    {
        Debug.Log("AirplaneTrip");
        CityBase air = GameManager.Inst.GameMap.GetPlace(MapID.Concord) as CityBase;
        air.PayUsePrice(player);
        GameManager.Inst.GameMap.Move(player, MapID.Taipei);
    }
    void Highway(Player player)
    {
        Debug.Log("Highway");
        GameManager.Inst.GameMap.Move(player, MapID.Start);
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
        foreach(var other in GameManager.Inst.Players)
        {
            if (other != player && other.Type != PlayerType.Bank)
            {
                other.Money -= 100;
                present += 100;
            }
        }
        player.Money += present;
    }
}
