using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GlodenKeyManager : MonoBehaviour
{
    System.Action[] keyUseFuncs;

    public System.Action this[GoldenKeyType keyType]
    {
        get => keyUseFuncs[(int)keyType];
    }

    public void Initialize()
    {
        int typeCount = System.Enum.GetValues(typeof(GoldenKeyType)).Length;
        keyUseFuncs = new System.Action[typeCount];
    }

    void ForcedSale(){}
    void IncomeTex(){}
    void RepairCost(){}
    void CrimePreventionCost(){}
    void IslandEscapeTicket(){}
    void FreePassTicket(){}
    void MoveBack(){}
    void Trip_Busan(){}
    void Trip_Jeju(){}
    void Trip_Seoul(){}
    void ToIsland(){}
    void GetFund(){}
    void RoundWorld(){}
    void MoveSpaceShip(){}
    void CruieTrip(){}
    void AirplaneTrip(){}
    void Highway(){}
    void NobelPrize(){}
    void LotteryWin(){}
    void RaceWin(){}
    void SchilarShip(){}
    void Pension(){}
    void StudyAbroad(){}
    void Hospital(){}
    void Fine(){}
    void Birthday() { }
}
