using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell_Enemy : Shell
{
    protected override void OnCollisionEnter(Collision collision)
    {
        // collision.contacts[0].normal;
        // collision.contacts[0].normal : 충돌한 면에 수직인 백터(노멀백터)
        onExplosion?.Invoke(transform, collision.contacts[0].normal);
        Destroy(this.gameObject);
    }
}
