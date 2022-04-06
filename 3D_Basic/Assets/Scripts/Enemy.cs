using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL = 0,
    CHASE,
    ATTACK
}

public class Enemy : MonoBehaviour
{
    public Transform[] waypoints = null;
    public float attackDelay = 1.0f;

    NavMeshAgent agent = null;
    Transform target = null;
    int waypointIndex = 0;
    int enterCounter = 0;
    int EnterCounter
    {
        get
        {
            return enterCounter;
        }
        set
        {
            enterCounter = Mathf.Clamp(value, 0, 2);
        }
    }
    float timeCount = 0.0f;    
    EnemyState state = EnemyState.PATROL;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.remainingDistance
    }

    private void Start()
    {
        agent.SetDestination(waypoints[waypointIndex].position);
    }

    bool CheckArrive()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    void GoNextWaypoint()
    {
        waypointIndex++;
        waypointIndex = waypointIndex % waypoints.Length;
        agent.SetDestination(waypoints[waypointIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnterCounter++;
            StateTransition(EnterCounter, other.gameObject);

            //Debug.Log($"대상 확인 : {other.name}");
            //agent.SetDestination(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnterCounter--;
            StateTransition(EnterCounter, other.gameObject);
        }
    }

    void StateTransition(int counter, GameObject _target)
    {
        state = (EnemyState)counter;

        switch (state)
        {
            case EnemyState.PATROL:
                target = null;
                agent.isStopped = false;
                // 원래 순찰 경로로 돌아가기
                break;
            case EnemyState.CHASE:
                target = _target.transform;
                agent.isStopped = false;
                break;
            case EnemyState.ATTACK:
                target = _target.transform;
                agent.isStopped = true;
                break;
            default:
                target = null;
                agent.isStopped = true;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.PATROL:
                if (CheckArrive())
                {
                    GoNextWaypoint();
                }
                break;
            case EnemyState.CHASE:
                agent.SetDestination(target.position);
                break;
            case EnemyState.ATTACK:
                timeCount += Time.fixedDeltaTime;
                if (timeCount > attackDelay)
                {
                    Debug.Log($"공격 : {Time.realtimeSinceStartup}");
                    timeCount = 0;
                }
                break;
            default:
                break;
        }
    }

    
    

    //void Test()
    //{
    //    agent.Warp(new Vector3(1,2,3)); // 순간이동 시킬때
    //    agent.speed;
    //    agent.acceleration;
    //    agent.radius;
    //    agent.avoidancePriority;
    //    agent.remainingDistance;
    //    agent.stoppingDistance;
    //    agent.autoRepath;
    //    agent.pathPending;              // 경로 계산중인지 아닌지 확인할 때
    //}
}
