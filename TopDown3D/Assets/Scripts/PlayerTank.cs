using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour, IControllable
{
    public float moveSpeed = 5.0f;
    public float spinSpeed = 20.0f;

    private Vector3 inputDir = Vector3.zero;
    public Vector3 KeyboardInputDir
    {
        set
        {
            inputDir = value;
            //Debug.Log($"Input : {inputDir}");
        }
    }

    private Rigidbody rigid = null;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigid.AddForce(inputDir.y * transform.forward * moveSpeed);
        rigid.AddTorque(0, inputDir.x * spinSpeed, 0);
    }
}
