using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour, IControllable
{
    public float moveSpeed = 10.0f;
    public float turnSpeed = 100.0f;
    Vector2 inputDir = Vector2.zero;
    Rigidbody rigid = null;

    public bool UseRigidbody { get => true; }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void AttackInput()
    {
    }

    public void ControllerConnect()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void InventoryOnOffInput()
    {
    }

    public void LockOnInput()
    {
    }

    public void MoveInput(Vector2 dir)
    {
        //dir.x;  // 좌회전 우회전
        //dir.y;  // 앞뒤
        inputDir = dir;
    }

    public void MoveUpdate()
    {
        rigid.MovePosition(rigid.position + inputDir.y * transform.forward * moveSpeed * Time.fixedDeltaTime);
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, inputDir.x * turnSpeed * Time.fixedDeltaTime, 0));
    }

    public void MoveModeChange()
    {
    }

    public void PickupInput()
    {
    }
}
