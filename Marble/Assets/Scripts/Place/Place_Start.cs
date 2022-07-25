using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Start : Place
{
    public int Salary = 200;

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Start);
        cover.transform.Rotate(0, 0, -90);
    }

    public void PaySalaray(Player player)
    {
        player.Money += Salary;
        Debug.Log($"{player} : 시작지점을 통과합니다.");
    }
}
