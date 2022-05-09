using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int hp = 0;
    public int maxHP = 3;
    public float moveSpeed = 1.0f;

    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        hp = maxHP;
        rigid.velocity = -this.transform.right * moveSpeed;
        rigid.angularVelocity = Random.Range(-360.0f, 360.0f); ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( !collision.gameObject.CompareTag("Border")
            && !collision.gameObject.CompareTag("Asteroid")
            && !collision.gameObject.CompareTag("Enemy") )
        {
            hp--;
            if(hp <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
