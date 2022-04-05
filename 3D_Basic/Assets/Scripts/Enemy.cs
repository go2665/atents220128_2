using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent = null;
    public Transform[] waypoints = null;
    int index = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.remainingDistance
    }

    private void Start()
    {
        agent.SetDestination(waypoints[index].position);
    }

    private void Update()
    {
        if(CheckArrive())
        {
            GoNextWaypoint();
        }
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
