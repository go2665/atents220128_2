using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject effect = null;
    public float speed = 5.0f;

    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        ResetVelocity();
    }

    private void ResetVelocity()
    {
        rigid.velocity = (-transform.right) * speed;
    }

    public void Die()
    {
        GameObject explosion = GameManager.Inst.GetExplosionObject();
        explosion.transform.position = transform.position;

        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Border"))
        {
            Die();
        }
    }
}
