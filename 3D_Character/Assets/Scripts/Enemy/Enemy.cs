using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE = 0,
    CHASE,
    ATTACK,
    DEAD
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
public class Enemy : MonoBehaviour, IBattle, IDie
{
    // HP
    private float hp = 100.0f;
    private float maxHP = 100.0f;
    
    // 공격
    private float attackPower = 10.0f;
    private float attackRange = 1.0f;
    private float attackSpeed = 1.0f;
    private float critical = 0.1f;

    // 방어
    private float defencePower = 10.0f;
    
    // 이동
    private float moveSpeed = 5.0f;
    
    // 상태
    private EnemyState state = EnemyState.IDLE;

    // 추적용 데이터
    public float waitTime = 3.0f;
    Transform playerTransform = null;
    NavMeshAgent navAgent = null;
    bool isChase = false;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = moveSpeed;

        SphereCollider col = GetComponent<SphereCollider>();
        col.radius = attackRange;
        col.isTrigger = true;
    }

    private void Start()
    {
        playerTransform = GameManager.Inst.MainPlayer.transform;
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.IDLE:
                IdleUpdate();
                break;
            case EnemyState.CHASE:
                ChaseUpdate();
                break;
            case EnemyState.ATTACK:
                AttackUpdate();
                break;
            case EnemyState.DEAD:
                Die();
                break;
            default:
                break;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            state = EnemyState.ATTACK;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            state = EnemyState.CHASE;
        }
    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = attackPower;
            if (Random.Range(0.0f, 1.0f) < critical)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defencePower;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        hp -= finalDamage;
        if( hp <= 0.0f )
        {
            Die();
        }
        hp = Mathf.Clamp(hp, 0.0f, maxHP);
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
        
    void IdleUpdate()
    {
        waitTime -= Time.deltaTime;
        if (waitTime < 0.0f)
        {
            state = EnemyState.CHASE;
        }
    }

    void ChaseUpdate()
    {
        navAgent.SetDestination(playerTransform.position);
    }

    private void AttackUpdate()
    {
        //쿨타임 체크
        //완료되면 Attack()실행
    }
}
