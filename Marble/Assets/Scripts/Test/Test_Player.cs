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
        City city = (City)map.GetPlace(MapID.Beijing);
        city.Sell(PlayerType.CPU1);

        //GameManager.Inst.UI_Manager.ShowBuildingPanel(true, p1, (City)map.GetPlace(MapID.Beijing));

        //map.Move(p1, MapID.Lisbon);
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
