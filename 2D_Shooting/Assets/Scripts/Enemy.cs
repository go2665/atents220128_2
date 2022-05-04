using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    public void ResetVelocity()
    {
        rigid.velocity = (-transform.right) * speed;
    }
}
