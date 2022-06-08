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

    float hp = 1.0f;        // 한대만 맞으면 사망하도록
    float maxHp = 1.0f;

    public float HP 
    { 
        get => hp; 
        set
        {
            hp = value;
            if( hp <= 0.0f )    // hp에 값을 설정할 때 hp가 0보다 작아지면 사망처리 함수 실행
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

    public void Dead()
    {
        GameObject obj = Instantiate(explosion);        // 탱크 폭팔용 이팩트 생성
        obj.transform.position = transform.position;    // 탱크 폭팔용 이팩트 배치
        obj.transform.rotation = transform.rotation;

        navAgent.isStopped = true;      // 길찾기 중단
        navAgent.enabled = false;       // 옆으로 구르기 위해 NavMeshAgent 끄기
        rigid.isKinematic = false;      // 물리 작용으로 움직이게 상태 변환
        rigid.constraints = RigidbodyConstraints.None;  // 특정 방향으로 이동하고 회전하는 것을 막아놓았던 것을 해제
        hitPoint.y = -3.0f;              // 폭팔력이 아래에서 위로 향하게 위치를 아래로 설정
        Vector3 dir = transform.position - hitPoint;    // 맞은 위치에서 맞은 탱크 위치로 가는 방향 백터
        rigid.AddForceAtPosition(dir * 5.0f, hitPoint, ForceMode.Impulse);  // dir 방향으로 힘 더하기
        rigid.AddTorque(dir * 10.0f, ForceMode.Impulse);    // dir 방향으로 회전력 더하기

        StartCoroutine(DeadProcess());  // 죽었을 때 일정 시간 이후 가라앉도록
    }

    IEnumerator DeadProcess()
    {
        yield return new WaitForSeconds(2.5f);  // 2.5초 동안 대기  
        SphereCollider[] spheres = GetComponents<SphereCollider>();
        foreach(var s in spheres)
        {
            s.enabled = false;  // 컬라이더 꺼서 바닥에 가라앉도록 처리
        }
    }
}
