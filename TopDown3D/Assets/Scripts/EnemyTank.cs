using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : MonoBehaviour
{
    public float fireAngle = 15.0f;
    public float waitTime = 1.0f;
    public GameObject shell = null;
    public Transform fireTransform = null;
    public float attackCoolTime = 3.0f;

    float halfFireAngle = 7.5f;
    NavMeshAgent navAgent = null;    
    bool chaseStart = false;
    float coolTime = 0.0f;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        halfFireAngle = fireAngle * 0.5f;
    }

    private void Start()
    {
        chaseStart = false;
        StartCoroutine(ChaseStart());
    }

    private void Update()
    {
        if(chaseStart)
        {
            navAgent.SetDestination(GameManager.Inst.MainPlayer.transform.position);
        }
        coolTime -= Time.deltaTime;
    }

    IEnumerator ChaseStart()
    {
        yield return new WaitForSeconds(waitTime);
        chaseStart = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 toTarget = other.transform.position - transform.position;
            if( Vector3.Angle(transform.forward, toTarget) < halfFireAngle )
            {
                if (coolTime < 0.0f)
                {
                    //Debug.Log("발사");
                    GameObject obj = Instantiate(shell);
                    obj.transform.position = fireTransform.position;
                    obj.transform.rotation = fireTransform.rotation;

                    coolTime = attackCoolTime;
                }
            }
        }        
    }
}
