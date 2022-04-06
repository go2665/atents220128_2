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
    int index = 0;
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
    EnemyState state = EnemyState.PATROL;
    float timeCount = 0.0f;    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.remainingDistance
    }

    private void Start()
    {
        agent.SetDestination(waypoints[index].position);
    }

    bool CheckArrive()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    void GoNextWaypoint()
    {
        index++;
        index = index % waypoints.Length;
        agent.SetDestination(waypoints[index].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnterCounter++;
            StateTransition(EnterCounter, other.transform);

            //Debug.Log($"대상 확인 : {other.name}");
            //agent.SetDestination(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnterCounter--;
            if (state == EnemyState.PATROL)
            {
                StateTransition(EnterCounter, null);
            }
            else
            {
                StateTransition(EnterCounter, other.transform);
            }
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

    void StateTransition(int counter, Transform _target)
    {
        state = (EnemyState)counter;
        target = _target;

        switch (state)
        {
            case EnemyState.PATROL:
                agent.isStopped = false;
                // 원래 순찰 경로로 돌아가기
                break;
            case EnemyState.CHASE:
                agent.isStopped = false;
                break;
            case EnemyState.ATTACK:
                agent.isStopped = true;
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
