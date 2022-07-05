using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_Start : Place
{
    public int Salary = 200;

    public void GetSalaray(Player player)
    {
        player.Money += Salary;
    }
}
