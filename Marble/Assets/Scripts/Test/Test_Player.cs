using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Player : MonoBehaviour
{
    void Start()
    {
        //Test_MaintenanceCost();        
        //Test_IslandEscapeTicket();
        //Test_FreepassTicket();
        //Test_Moveback();
        //Test_Trip();
        //Test_ToIsland();
        //Test_Fund();
        //Test_RoundWorld();
        //Test_MoveSpaceShip();
        //Test_Cruise();
        //Test_Airplane();
        //Test_Highway();
        //Test_Birthday();

        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        //map.Move(p1, MapID.GoldenKey1-3);
    }

    private static void Test_Birthday()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        map.Move(p1, MapID.GoldenKey6);
        gkManager.RunGoldenCard(GoldenKeyType.Birthday, p1);
    }

    private static void Test_Highway()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        map.Move(p1, MapID.GoldenKey1);
        gkManager.RunGoldenCard(GoldenKeyType.Highway, p1);
    }

    private static void Test_Airplane()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);

        CityBase city = (CityBase)map.GetPlace(MapID.Concord);
        city.Sell(PlayerType.CPU1);
        p1.Money = 0;
        p2.Money = 0;

        map.Move(p1, MapID.GoldenKey6);
        gkManager.RunGoldenCard(GoldenKeyType.AirplaneTrip, p1);
    }
    
    private static void Test_Cruise()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);

        CityBase city = (CityBase)map.GetPlace(MapID.QueenElizabeth);
        city.Sell(PlayerType.CPU1);
        p1.Money = 0;
        p2.Money = 0;

        map.Move(p1, MapID.GoldenKey6);
        gkManager.RunGoldenCard(GoldenKeyType.CruiseTrip, p1);
    }

    private static void Test_MoveSpaceShip()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        gkManager.RunGoldenCard(GoldenKeyType.MoveSpaceShip, p1);
    }

    private static void Test_RoundWorld()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);
        p1.Money = 0;
        p2.Money = 0;
        p3.Money = 0;
        p4.Money = 0;
        map.Move(p2, MapID.Fund_Pay);
        map.Move(p3, MapID.Fund_Pay);
        map.Move(p4, MapID.Fund_Pay);
        gkManager.RunGoldenCard(GoldenKeyType.RoundWorld, p1);
    }

    private static void Test_Fund()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);
        p1.Money = 0;
        p2.Money = 0;
        p3.Money = 0;
        p4.Money = 0;
        map.Move(p1, MapID.Fund_Pay);
        map.Move(p2, MapID.Fund_Pay);
        map.Move(p3, MapID.Fund_Pay);
        gkManager.RunGoldenCard(GoldenKeyType.GetFund, p4);
    }

    private static void Test_ToIsland()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        p1.Money = 0;
        map.SetPosition(p1, MapID.GoldenKey6);
        gkManager.RunGoldenCard(GoldenKeyType.ToIsland, p1);
    }

    private static void Test_Trip()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);

        map.SetPosition(p1, MapID.Seoul);
        p1.Money = 0;
        p2.Money = 0;
        p3.Money = 0;

        gkManager.RunGoldenCard(GoldenKeyType.Trip_Busan, p1);
        gkManager.RunGoldenCard(GoldenKeyType.Trip_Jeju, p2);
        gkManager.RunGoldenCard(GoldenKeyType.Trip_Seoul, p3);
    }

    private static void Test_Moveback()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);
        map.SetPosition(p1, MapID.GoldenKey1);
        map.SetPosition(p2, MapID.GoldenKey2);
        map.SetPosition(p3, MapID.GoldenKey3);
        map.SetPosition(p4, MapID.GoldenKey4);
        gkManager.RunGoldenCard(GoldenKeyType.MoveBack, p1);
        gkManager.RunGoldenCard(GoldenKeyType.MoveBack, p2);
        gkManager.RunGoldenCard(GoldenKeyType.MoveBack, p3);
        gkManager.RunGoldenCard(GoldenKeyType.MoveBack, p4);
        map.SetPosition(p3, MapID.GoldenKey5);
        map.SetPosition(p4, MapID.GoldenKey6);
        gkManager.RunGoldenCard(GoldenKeyType.MoveBack, p3);
        gkManager.RunGoldenCard(GoldenKeyType.MoveBack, p4);
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
