using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_BadEffect : Shell
{
    public GameObject badEffect = null;

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        GameObject obj = Instantiate(badEffect);
        obj.transform.position = collision.contacts[0].point;
    }
}
