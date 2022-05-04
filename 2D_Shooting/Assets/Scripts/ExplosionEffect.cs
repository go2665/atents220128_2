using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    Animator anim = null;
    int hash_explosion = Animator.StringToHash("Explosion");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.Play(hash_explosion);
    }
}
