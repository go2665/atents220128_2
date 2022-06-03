using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public ShellData data = null;

    public delegate void ExplosionDelegate(Transform objTransform, Vector3 up);
    public ExplosionDelegate onExplosion = null;

    protected Rigidbody rigid = null;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        onExplosion = data.Explosion;
    }

    protected virtual void Start()
    {
        rigid.velocity = transform.forward * data.initialSpeed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // collision.contacts[0].normal;
        // collision.contacts[0].normal : 충돌한 면에 수직인 백터(노멀백터)
        onExplosion?.Invoke(transform, collision.contacts[0].normal);
        Destroy(this.gameObject);
    }
}
