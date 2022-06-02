using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public float initialSpeed = 20.0f;
    public GameObject explosionEffect = null;

    protected Rigidbody rigid = null;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        rigid.velocity = transform.forward * initialSpeed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // collision.contacts[0].normal;
        // collision.contacts[0].normal : 충돌한 면에 수직인 백터(노멀백터)
        Explotion(collision.contacts[0].normal);
    }

    protected virtual void Explotion(Vector3 up)
    {
        GameObject effect = Instantiate(explosionEffect);
        effect.transform.position = transform.position;
                
        // Quaternion.AngleAxis(-90.0f, transform.right) : 이펙트 자체가 회전되어 있어서 추가로 각도 맞춤
        // Quaternion.LookRotation(forward, collision.contacts[0].normal) : 충돌한 면의 노멀백터가 up백터가 되는 회전 만들기
        Vector3 forward = Quaternion.AngleAxis(90.0f, transform.right) * up;
        effect.transform.rotation =
            Quaternion.AngleAxis(-90.0f, transform.right) * Quaternion.LookRotation(forward, up);
        Destroy(this.gameObject);
    }
}
