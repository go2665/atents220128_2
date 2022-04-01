using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Stay : Turret_Base
{
    // 시작하자 마자 일정간격으로 총을 쏘는 터렛
    private void Start()
    {
        Initiallize();
        StartFire();
    }
}
