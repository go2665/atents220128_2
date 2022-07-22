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
        //Test_Bankrupt();


        //Map map = GameManager.Inst.GameMap;
        //Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);

        //CityBase cityBase = (CityBase)map.GetPlace(MapID.Jeju);
        //cityBase.Sell(PlayerType.Human);

        //map.Move(p1, MapID.Jeju - 4);

        Test_Island_Normal();

    }

    private static void Test_Island_Normal()
    {
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        p1.TestDice(6, 4);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        p2.TestDice(1, 3);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        p3.TestDice(1, 3);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);
        p4.TestDice(1, 3);
    }

    private static void Test_Bankrupt()
    {
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);

        CityBase cityBase = (CityBase)map.GetPlace(MapID.Seoul);
        cityBase.Sell(PlayerType.CPU1);

        City city = null;

        //city = (City)map.GetPlace(MapID.NewYork);
        //city.Sell(PlayerType.Human);
        //city.MakeBuildings(new int[] { 1, 1, 1 });

        //city = (City)map.GetPlace(MapID.London);
        //city.Sell(PlayerType.Human);
        //city.MakeBuildings(new int[] { 1, 1, 1 });

        //city = (City)map.GetPlace(MapID.Athens);
        //city.Sell(PlayerType.Human);
        //city.MakeBuildings(new int[] { 1, 1, 1 });

        //city = (City)map.GetPlace(MapID.Hawaii);
        //city.Sell(PlayerType.Human);
        //city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Taipei);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Singapore);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Cairo);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        cityBase = (CityBase)map.GetPlace(MapID.Jeju);
        cityBase.Sell(PlayerType.Human);

        cityBase = (CityBase)map.GetPlace(MapID.Concord);
        cityBase.Sell(PlayerType.Human);

        p1.Money = 0;
        p1.Move(MapID.Seoul - 3);
    }

    private void Update()
    {
        //Update_Test_MaintenanceCost();
        if( Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
            p1.Money = 100000;
        }
    }


    private static void Test_Birthday()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        p1.Move(MapID.GoldenKey6);
        gkManager.RunGoldenCard(GoldenKeyType.Birthday, p1);
    }

    private static void Test_Highway()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        p1.Move(MapID.GoldenKey1);
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

        p1.Move(MapID.GoldenKey6);
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

        p1.Move(MapID.GoldenKey6);
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
        p2.Move(MapID.Fund_Pay);
        p3.Move(MapID.Fund_Pay);
        p4.Move(MapID.Fund_Pay);
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
        p1.Move(MapID.Fund_Pay);
        p2.Move(MapID.Fund_Pay);
        p3.Move(MapID.Fund_Pay);
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

        p1.Move(MapID.NewYork - 3);
        p3.Move(MapID.NewYork - 3);
    }

    private static void Test_IslandEscapeTicket()
    {
        GoldenKeyManager gkManager = GameManager.Inst.GoldenKeyManager;
        Map map = GameManager.Inst.GameMap;
        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        gkManager.RunGoldenCard(GoldenKeyType.IslandEscapeTicket, p2);
        p2.Move(MapID.GoldenKey2);
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
