using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player : MonoBehaviour
{
    void Start()
    {
        Player[] players = GameManager.Inst.Players;
        Map map = GameManager.Inst.GameMap;

        Player p1 = GameManager.Inst.GetPlayer(PlayerType.Human);
        Player p2 = GameManager.Inst.GetPlayer(PlayerType.CPU1);
        Player p3 = GameManager.Inst.GetPlayer(PlayerType.CPU2);
        Player p4 = GameManager.Inst.GetPlayer(PlayerType.CPU3);

        

        City city = (City)map.GetPlace(MapID.NewYork);
        city.Sell(PlayerType.Human);
        city.MakeBuildings(new int[] { 1, 1, 2 });

        city = (City)map.GetPlace(MapID.London);
        city.Sell(PlayerType.CPU1);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Istanbul);
        city.Sell(PlayerType.CPU2);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        city = (City)map.GetPlace(MapID.Hawaii);
        city.Sell(PlayerType.CPU3);
        city.MakeBuildings(new int[] { 1, 1, 1 });

        CityBase cityBase = (CityBase)map.GetPlace(MapID.Jeju);
        //cityBase.Sell(PlayerType.Human);

        city = (City)map.GetPlace(MapID.Paris);
        city.Sell(PlayerType.Human);

        city = (City)map.GetPlace(MapID.Ottawa);
        city.Sell(PlayerType.Human);

        city = (City)map.GetPlace(MapID.Madrid);
        city.Sell(PlayerType.CPU3);

        city = (City)map.GetPlace(MapID.BuenosAires);
        city.Sell(PlayerType.CPU2);

        city = (City)map.GetPlace(MapID.Bern);
        city.Sell(PlayerType.CPU1);

        city = (City)map.GetPlace(MapID.Beijing);
        city.Sell(PlayerType.CPU1);

        city = (City)map.GetPlace(MapID.Manila);
        city.Sell(PlayerType.CPU1);

        city = (City)map.GetPlace(MapID.Cairo);
        city.Sell(PlayerType.CPU1);

        Debug.Log( map.TravelRiskCheck(p2));

        p1.Money = 100;
        p2.Money = 100;
        p3.Money = 100;
        p4.Money = 100;

        //List<CityBase> test = new List<CityBase>();
        //test.Remove(city);

        //GameManager.Inst.UI_Manager.ShowBuildingPanel(true, p1, (City)map.GetPlace(MapID.Beijing));

        //map.Move(p2, MapID.Lisbon);
        //map.Move(p3, MapID.Lisbon);
        //map.Move(p4, MapID.Lisbon);


        //p1.Money = 100;

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
}
