using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public delegate void ReturnToPoolDelegate(GameObject obj);
    public ReturnToPoolDelegate onReturnToPool { get; set; }

    private void OnDisable()
    {
        onReturnToPool?.Invoke(this.gameObject);
    }
}
