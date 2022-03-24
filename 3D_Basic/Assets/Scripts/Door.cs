using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Open()
    {
        anim.SetTrigger("Open");
    }

    void Close()
    {
        anim.SetTrigger("Close");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close();

            
        }
    }
}
