using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDead deadTarget = other.GetComponent<IDead>();
        if(deadTarget!=null)
        {
            deadTarget.OnDead();
        }
    }
}
