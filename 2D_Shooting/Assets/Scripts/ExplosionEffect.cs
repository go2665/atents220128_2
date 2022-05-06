using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    Animator anim = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if( anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f )
        {
            this.gameObject.SetActive(false);
        }
    }
}
