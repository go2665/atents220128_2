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
public class Enemy : MonoBehaviour, IBattle, IDie, IHealth
{
    // IHealth
    private float hp = 100.0f;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            onHealthChange?.Invoke();
        }
    }
    private float maxHP = 100.0f;
    public float MaxHP
    {
        get
        {
            return maxHP;
        }
    }
    public HealthDelegate onHealthChange { get; set; }

    // 공격
    private float attackPower = 10.0f;
    private float attackRange = 1.0f;
    private float attackSpeed = 1.0f;
    private float attackCooltime = 1.0f;
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

    // 공격용 데이터
    IBattle playerBattle = null;

    // 피격 처리용 데이터
    public Material hit = null;
    Material original = null;
    SkinnedMeshRenderer skRenderer = null;
    float changeDuration = 0.5f;
    float hitElapsed = 0.0f;
    Collider bodyCollider = null;
    //HealthBar healthBar = null;

    // 기타
    Rigidbody rigid = null;
    Animator anim = null;


    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = moveSpeed;

        SphereCollider col = GetComponent<SphereCollider>();
        col.radius = attackRange;
        col.isTrigger = true;

        skRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        original = skRenderer.material;         
        bodyCollider = transform.Find("HitBox").GetComponent<Collider>();
        //healthBar = GetComponentInChildren<HealthBar>();
        //healthBar.SetMaxHealth(maxHP);

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        playerTransform = GameManager.Inst.MainPlayer.transform;
        playerBattle = GameManager.Inst.MainPlayer.GetComponent<IBattle>();

        HP = maxHP;
    }

    private void Update()
    {
        anim.SetInteger("EnemyState", (int)state);

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
                break;
            default:
                break;
        }  
        
        if( skRenderer.material != original )
        {
            Color newColor = skRenderer.material.color;
            //newColor = Color.Lerp(newColor, Color.white, changeDuration * Time.deltaTime);

            // Mathf.Sin()               //Sin함수
            // Mathf.Deg2Rad * 90.0f;    //Degree를 Radian으로 변경해주는 상수
            // Mathf.Rad2Deg * 2;        //Radian을 Degree로 변경해주는 상수

            // 270~360로 변경되는 수를 만들기
            // 처음 맞았을 때 270, changeDuration만큼 시간이 지났을 때 360
            // float angle = 270.0f;
            hitElapsed += Time.deltaTime;
            // Debug.Log(hitElapsed);

            // angle값은 hitElapsed가 0일때 = 270, hitElapsed가 changeDuration일 때 360
            // angle값은 진행상황이 0%일 때 270, 100%일 때 360
            // Debug.Log($"Normal hitElapsed : {hitElapsed/changeDuration}");
            // hitElapsed/changeDuration는 0~1사이로 값이 변경된다.
            // Debug.Log($"Degree : {270.0f + hitElapsed / changeDuration * 90.0f}");
            // Debug.Log($"Sin(270~360) : {Mathf.Sin((270.0f + hitElapsed / changeDuration * 90.0f) * Mathf.Deg2Rad)}");
            // -1 ~ 0 => 0 ~ 1
            // Debug.Log($"Sin(270~360) + 1 : {Mathf.Sin((270.0f + hitElapsed / changeDuration * 90.0f) * Mathf.Deg2Rad)+1.0f}");

            newColor.a = Mathf.Sin((270.0f + hitElapsed / changeDuration * 90.0f) * Mathf.Deg2Rad) + 1.0f;
            skRenderer.material.color = newColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            state = EnemyState.ATTACK;
            //StartCoroutine(TransitionToAttack());
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
            attackCooltime = attackSpeed;
        }
    }

    IEnumerator TransitionToAttack()
    {
        yield return new WaitForSeconds(0.1f);
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        if (state != EnemyState.DEAD && other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {            
            navAgent.isStopped = false;           
            state = EnemyState.CHASE;
        }
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
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
        Debug.Log($"{gameObject.name} : {finalDamage} 데미지 입음");
        HP -= finalDamage;
        if (HP <= 0.0f)
        {
            Die();
        }
        else
        {
            HP = Mathf.Clamp(HP, 0.0f, maxHP);
            anim.SetTrigger("Hit");
            attackCooltime = attackSpeed;
            StartCoroutine(UnBeatable());            
        }
    }

    IEnumerator UnBeatable()
    {
        bodyCollider.enabled = false;
        skRenderer.material = hit;
        skRenderer.material.color = new Color(1, 1, 1, 0);
        hitElapsed = 0.0f;
        yield return new WaitForSeconds(changeDuration);
        skRenderer.material = original;
        bodyCollider.enabled = true;
    }

    public virtual void Die()
    {
        HP = 0.0f;

        state = EnemyState.DEAD;

        GameManager.Inst.MainPlayer.LockOff(bodyCollider.transform);

        bodyCollider.enabled = false;

        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;

        anim.SetTrigger("Die");

        Destroy(this.gameObject, 3.0f);
    }  

    private void IdleUpdate()
    {        
        waitTime -= Time.deltaTime;
        if (waitTime < 0.0f)
        {
            state = EnemyState.CHASE;
        }
    }

    private void ChaseUpdate()
    {
        navAgent.SetDestination(playerTransform.position);
    }

    private void AttackUpdate()
    {
        attackCooltime -= Time.deltaTime;
        if(attackCooltime < 0.0f)
        {
            anim.SetTrigger("Attack");
            Attack(playerBattle);
            attackCooltime = attackSpeed;
        }
    }

    //public float GetHealth()
    //{
    //    return hp;
    //}

    //public float GetMaxHealth()
    //{
    //    return maxHP;
    //}
}
