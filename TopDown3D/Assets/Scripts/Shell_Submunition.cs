using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Submunition : Shell
{
    protected override void Start()
    {
    }

    public void RandomSpread(Vector3 downDir)
    {
        Vector2 random = Random.insideUnitCircle;
        Vector3 temp = (downDir + new Vector3(random.x, 0, random.y)).normalized * initialSpeed;
        rigid.velocity = temp;
        Debug.Log($"{gameObject.name} : {rigid.velocity}");
    }
}
