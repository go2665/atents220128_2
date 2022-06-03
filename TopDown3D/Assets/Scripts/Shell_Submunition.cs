using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Submunition : Shell
{
    protected override void Start()
    {
        // Shell의 Start는 실행 안함(초기속도 설정하면 안됨)
    }

    public void RandomSpread(Vector3 downDir)
    {
        Vector2 random = Random.insideUnitCircle;
        Vector3 temp = (downDir + new Vector3(random.x, 0, random.y)).normalized * data.initialSpeed;
        rigid.velocity = temp;
        //Debug.Log($"{gameObject.name} : {rigid.velocity}");
    }
}
