using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : MonoBehaviour, IHealth
{
    public float fireAngle = 15.0f;
    public float waitTime = 1.0f;
    public GameObject shell = null;
    public Transform fireTransform = null;
    public float attackCoolTime = 3.0f;
    public GameObject explosion = null;

    NavMeshAgent navAgent = null;
    Rigidbody rigid = null;

    float halfFireAngle = 7.5f;
    bool chaseStart = false;
    float coolTime = 0.0f;

    float hp = 1.0f;
    float maxHp = 1.0f;
    bool isDead = false;

    public float HP 
    { 
        get => hp; 
        set
        {
            hp = value;
            if( hp <= 0.0f )
            {
                Dead();
            }
        }
    }

    public float MaxHP { get => maxHp; }

    Vector3 hitPoint = Vector3.zero;
    public Vector3 HitPoint { set => hitPoint = value; }

    public IHealth.HealthDelegate onHealthChange { get; set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
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
        if(chaseStart && !isDead)
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

    public void Dead()
    {
        GameObject obj = Instantiate(explosion);
        obj.transform.position = this.transform.position;
        obj.transform.rotation = this.transform.rotation;

        navAgent.isStopped = true;
        navAgent.enabled = false;
        rigid.isKinematic = false;
        rigid.constraints = RigidbodyConstraints.None;
        hitPoint.y = 0.0f;
        rigid.AddForceAtPosition((transform.position - hitPoint) * 5.0f, hitPoint, ForceMode.Impulse);
        rigid.AddTorque(Quaternion.Euler(0, 90, 0) * ((transform.position - hitPoint) * 10.0f), ForceMode.Impulse);
        isDead = true;

        StartCoroutine(DeadProcess());
    }

    IEnumerator DeadProcess()
    {
        yield return new WaitForSeconds(3.0f);        
        SphereCollider[] spheres = GetComponents<SphereCollider>();
        foreach(var s in spheres)
        {
            s.enabled = false;
        }
    }
}
