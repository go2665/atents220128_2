using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Player : MonoBehaviour
{
    void Start()
    {
        //Test_MaintenanceCost();        
        Test_IslandEscapeTicket();
        //Test_FreepassTicket();



    }

    private static void Test_FreepassTicket()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);

        City city = (City)map.GetPlace(MapID.NewYork);
        city.Sell(PlayerType.CPU1);
        city.MakeBuildings(new int[] { 1, 1, 1 });
        p1.Money = 0;
        p2.Money = 0;
        p3.Money = 0;

        gkManager.RunGoldenCard(GoldenKeyType.FreePassTicket, p3);

        map.Move(p1, MapID.NewYork - 3);
        map.Move(p3, MapID.NewYork - 3);
    }

    private static void Test_IslandEscapeTicket()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        gkManager.RunGoldenCard(GoldenKeyType.IslandEscapeTicket, p2);
        map.Move(p2, MapID.GoldenKey2);
    }

    private void Update()
    {
        //Update_Test_MaintenanceCost();
    }

    private static void Test_MaintenanceCost()
    {
        Player[] players = GameManager.Inst.Players;
        Map map = GameManager.Inst.GameMap;

        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);

        City city = (City)map.GetPlace(MapID.NewYork);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.London);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Istanbul);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Hawaii);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        CityBase cityBase = (CityBase)map.GetPlace(MapID.Seoul);
        cityBase.Sell(PlayerType.Human);

        //List<CityBase> test = new List<CityBase>();
        //test.Remove(city);

        //GameManager.Inst.UI_Manager.ShowBuildingPanel(true, p1, (City)map.GetPlace(MapID.Beijing));

        //map.Move(p2, MapID.Lisbon);
        //map.Move(p3, MapID.Lisbon);
        //map.Move(p4, MapID.Lisbon);


        p1.Money = 0;

        //GameManager.Inst.UI_Manager.SetPlaceInfo(MapID.Taipei);

        //map.Move(GameManager.Inst.GetPlayer(PlayerType.Human), 1);
        //map.Move(GameManager.Inst.GetPlayer(PlayerType.CPU1), 2);
        //map.Move(GameManager.Inst.GetPlayer(PlayerType.CPU2), 3);
        //map.Move(GameManager.Inst.GetPlayer(PlayerType.CPU3), 4);
        //GameManager.Inst.TurnManager.TurnProcess();
        //GameManager.Inst.TurnManager.TurnProcess();
        //GameManager.Inst.TurnManager.TurnProcess();
        //GameManager.Inst.TurnManager.TurnProcess();
    }

    private void Update_Test_MaintenanceCost()
    {
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            gkManager.RunGoldenCard(GoldenKeyType.IncomeTex, p1);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            gkManager.RunGoldenCard(GoldenKeyType.RepairCost, p1);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            gkManager.RunGoldenCard(GoldenKeyType.CrimePreventionCost, p1);
        }
    }
}
