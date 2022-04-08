using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    GameObject child = null;

    private void Awake()
    {
        child = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        child.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // 점수를 추가시킨다.
            GameManager.Inst.CoinCount++;
        }
    }
}
