using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GlodenKeyManager : MonoBehaviour
{    
    System.Action[] keyUseFuncs;
    List<GoldenKey> shuffle;

    public System.Action this[GoldenKeyType keyType]
    {
        get => keyUseFuncs[(int)keyType];
    }

    public void Initialize()
    {
        int typeCount = System.Enum.GetValues(typeof(GoldenKeyType)).Length;
        keyUseFuncs = new System.Action[typeCount];
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
        keyUseFuncs[(int)GoldenKeyType.CruieTrip] = CruieTrip;
        keyUseFuncs[(int)GoldenKeyType.AirplaneTrip] = AirplaneTrip;
        keyUseFuncs[(int)GoldenKeyType.Highway] = Highway;
        keyUseFuncs[(int)GoldenKeyType.NobelPrize] = NobelPrize;
        keyUseFuncs[(int)GoldenKeyType.LotteryWin] = LotteryWin;
        keyUseFuncs[(int)GoldenKeyType.RaceWin] = RaceWin;
        keyUseFuncs[(int)GoldenKeyType.SchilarShip] = SchilarShip;
        keyUseFuncs[(int)GoldenKeyType.Pension] = Pension;
        keyUseFuncs[(int)GoldenKeyType.StudyAbroad] = StudyAbroad;
        keyUseFuncs[(int)GoldenKeyType.Hospital] = Hospital;
        keyUseFuncs[(int)GoldenKeyType.Fine] = Fine;
        keyUseFuncs[(int)GoldenKeyType.Birthday] = Birthday;

        // GoldenKeyType을 하나씩 넣은 다음 랜덤하게 섞어 shuffle을 채우기.
        //shuffle;
    }

    void ForcedSale() { }
    void IncomeTex() { }
    void RepairCost() { }
    void CrimePreventionCost() { }
    void IslandEscapeTicket() { }
    void FreePassTicket() { }
    void MoveBack() { }
    void Trip_Busan() { }
    void Trip_Jeju() { }
    void Trip_Seoul() { }
    void ToIsland() { }
    void GetFund() { }
    void RoundWorld() { }
    void MoveSpaceShip() { }
    void CruieTrip() { }
    void AirplaneTrip() { }
    void Highway() { }
    void NobelPrize() { }
    void LotteryWin() { }
    void RaceWin() { }
    void SchilarShip() { }
    void Pension() { }
    void StudyAbroad() { }
    void Hospital() { }
    void Fine() { }
    void Birthday() { }
}
