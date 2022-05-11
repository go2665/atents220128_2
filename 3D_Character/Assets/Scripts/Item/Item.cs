using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    public ItemData data = null;

    protected SphereCollider trigger = null;

    private void Awake()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;
    }

    protected virtual void Start()
    {
        trigger.radius = data.triggerSize;
        Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }
}
