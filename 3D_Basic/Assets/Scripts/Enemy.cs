using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent = null;
    public Transform startPoint = null;
    public Transform endPoint = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.remainingDistance
    }

    private void Start()
    {
        agent.SetDestination(startPoint.position);
    }
}
