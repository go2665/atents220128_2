using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoldenKeyManager : MonoBehaviour
{    
    System.Action[] keyUseFuncs;
    //System.Action[] shuffleArray;
    List<GoldenKeyType> shuffle;

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
        shuffle = Shuffle();
        //shuffleArray = Shuffle(keyUseFuncs);
    }

    public void RunGoldenCard(GoldenKeyType type)
    {
        this[type]?.Invoke();
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

    void ForcedSale() 
    {
        Debug.Log("ForcedSale");
    }
    void IncomeTex() 
    {
        Debug.Log("IncomeTex");
    }
    void RepairCost()
    {
        Debug.Log("RepairCost");
    }
    void CrimePreventionCost()
    {
        Debug.Log("CrimePreventionCost");
    }
    void IslandEscapeTicket()
    {
        Debug.Log("IslandEscapeTicket");
    }
    void FreePassTicket()
    {
        Debug.Log("FreePassTicket");
    }
    void MoveBack()
    {
        Debug.Log("MoveBack");
    }
    void Trip_Busan()
    {
        Debug.Log("Trip_Busan");
    }
    void Trip_Jeju()
    {
        Debug.Log("Trip_Jeju");
    }
    void Trip_Seoul()
    {
        Debug.Log("Trip_Seoul");
    }
    void ToIsland()
    {
        Debug.Log("ToIsland");
    }
    void GetFund()
    {
        Debug.Log("GetFund");
    }
    void RoundWorld()
    {
        Debug.Log("RoundWorld");
    }
    void MoveSpaceShip()
    {
        Debug.Log("MoveSpaceShip");
    }
    void CruieTrip()
    {
        Debug.Log("CruieTrip");
    }
    void AirplaneTrip()
    {
        Debug.Log("AirplaneTrip");
    }
    void Highway()
    {
        Debug.Log("Highway");
    }
    void NobelPrize()
    {
        Debug.Log("NobelPrize");
    }
    void LotteryWin()
    {
        Debug.Log("LotteryWin");
    }
    void RaceWin()
    {
        Debug.Log("RaceWin");
    }
    void SchilarShip()
    {
        Debug.Log("SchilarShip");
    }
    void Pension()
    {
        Debug.Log("Pension");
    }
    void StudyAbroad()
    {
        Debug.Log("StudyAbroad");
    }
    void Hospital()
    {
        Debug.Log("Hospital");
    }
    void Fine()
    {
        Debug.Log("Fine");
    }
    void Birthday()
    {
        Debug.Log("Birthday");
    }
}
