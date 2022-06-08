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

        // 맞은 대상에게서 IHealth 인터페이스가 있는지 확인
        IHealth health = collision.gameObject.GetComponent<IHealth>();
        if(health != null)
        {
            // IHealth 인터페이스를 가지고 있으면 피격위치 기록 및 HP 감소
            health.HitPoint = collision.GetContact(0).point;
            health.HP -= data.damage;
        }

        // 포탄 사라짐
        Destroy(this.gameObject);
    }
}
