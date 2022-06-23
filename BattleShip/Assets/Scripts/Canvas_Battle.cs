using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Canvas_Battle : MonoBehaviour
{
    TextMeshProUGUI playerPoint = null;
    TextMeshProUGUI enemyPoint = null;

    private void Awake()
    {
        playerPoint = transform.Find("PlayerPoint").GetComponent<TextMeshProUGUI>();
        enemyPoint = transform.Find("EnemyPoint").GetComponent<TextMeshProUGUI>();        
    }

    private void Start()
    {
        GameManager.Inst.FieldLeft.OnAliveShipCountChange += RefreshPlayerPoint;
        GameManager.Inst.FieldRight.OnAliveShipCountChange += RefreshEnemyPoint;
    }

    void RefreshPlayerPoint(int point)
    {
        playerPoint.text = point.ToString();
    }

    void RefreshEnemyPoint(int point)
    {
        enemyPoint.text = point.ToString();
    }
}
