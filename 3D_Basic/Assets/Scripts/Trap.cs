using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator anim = null;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 내 게임 오브젝트가 가지고 있는 트리거로 설정된 컬라이더에 다른 컬라이더가 들어왔을 때 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}가 함정을 밟음");
            anim.SetTrigger("TrapActive");

            IDead dead = other.gameObject.GetComponent<IDead>();
            if(dead !=null)
            { 
                dead.OnDead();
            }            
        }
    }

    // 내 게임 오브젝트가 가지고 있는 트리거로 설정된 컬라이더에 다른 컬라이더가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}가 함정에서 나감");
            anim.SetTrigger("TrapDeactivate");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);        
    }

}
