using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data = null;

    private void Awake()
    {
        Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }
}
