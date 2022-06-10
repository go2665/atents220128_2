using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    TextMesh text = null;
    float countTime;
    public float CountTime { set => countTime = value; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMesh>();
    }

    private void Start()
    {
        transform.LookAt(transform.parent.position + Vector3.forward);        
    }

    private void Update()
    {
        countTime -= Time.deltaTime;
        if (countTime < 0.0f)        
        {
            countTime = 0.0f;
            this.gameObject.SetActive(false);
        }
        text.text = $"{countTime:f1}";
    }
}
