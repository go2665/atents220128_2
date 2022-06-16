using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player : MonoBehaviour
{
    public Ship[] ships;
    public BattleField enemyField;
    public Player player;

    void Start()
    {
        //Test_Attack();
        StartCoroutine(StartDelay());
        
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Test_Attack();
    }

    private void Test_Attack()
    {
        enemyField.ShipDeployment(new Vector2Int(0, 0), ships[0]);
        enemyField.ShipDeployment(new Vector2Int(3, 3), ships[1]);
        enemyField.ShipDeployment(new Vector2Int(5, 5), ships[2]);
        enemyField.ShipDeployment(new Vector2Int(7, 7), ships[3]);
        ships[4].Rotate();
        enemyField.ShipDeployment(new Vector2Int(9, 9), ships[4]);

        player.enemyField = enemyField;
        //player.Attack(new Vector2Int(0, 0));    // 공격 받음
        //player.Attack(new Vector2Int(1, 0));    // 바다
        //player.Attack(new Vector2Int(0, 1));    // 공격 받음
        //player.Attack(new Vector2Int(0, 2));    // 공격 받음
        //player.Attack(new Vector2Int(0, 3));    // 공격 받음
        //player.Attack(new Vector2Int(0, 4));    // 공격 받음
        //player.Attack(new Vector2Int(0, 5));    // 바다

        //for (int i = 0; i < 100; i++)
        //{
        //    player.ForcedAttack();
        //}
    }


}
