using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_BadEffect : Shell
{

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        GameObject obj = Instantiate((data as ShellData_BadEffect).badEffect);
        obj.transform.position = collision.contacts[0].point;
        obj.transform.Translate(Vector3.up * 0.25f, Space.World);
    }
}
