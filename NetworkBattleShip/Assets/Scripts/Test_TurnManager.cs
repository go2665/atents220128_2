using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TurnManager : MonoBehaviour
{
    public Ship[] playerShips;
    public Ship[] enemyShips;
    public BattleField playerField;
    public BattleField enemyField;
    public Player player;
    public Player enemy;

    private void Start()
    {
        Test_Turn();
    }

    private void Update()
    {
        //if( Keyboard.current.digit1Key.wasPressedThisFrame )
        //{
        //    player.ForcedAttack();
        //}
        //if (Keyboard.current.digit2Key.wasPressedThisFrame)
        //{
        //    enemy.ForcedAttack();
        //}
    }

    void Test_Turn()
    {
        //player.Initialize(playerField, enemyField);
        //enemy.Initialize(enemyField, playerField);

        //enemyField.ShipDeployment(new Vector2Int(0, 0), enemyShips[0]);
        //enemyField.ShipDeployment(new Vector2Int(1, 0), enemyShips[1]);
        //enemyField.ShipDeployment(new Vector2Int(2, 0), enemyShips[2]);
        //enemyField.ShipDeployment(new Vector2Int(3, 0), enemyShips[3]);
        //enemyField.ShipDeployment(new Vector2Int(4, 0), enemyShips[4]);

        //playerField.ShipDeployment(new Vector2Int(0, 0), playerShips[0]);
        //playerField.ShipDeployment(new Vector2Int(1, 0), playerShips[1]);
        //playerField.ShipDeployment(new Vector2Int(2, 0), playerShips[2]);
        //playerField.ShipDeployment(new Vector2Int(3, 0), playerShips[3]);
        //playerField.ShipDeployment(new Vector2Int(4, 0), playerShips[4]);
    }
}
