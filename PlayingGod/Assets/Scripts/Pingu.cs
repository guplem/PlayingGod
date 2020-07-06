using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pingu : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 home;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        home = transform.position;
    }

    private void Update()
    {
        if (DestinationReached())
        {
            navMeshAgent.destination = GameManager.instance.GetRandomPointInScenario(0.1f);
        }
    }

    public bool DestinationReached()
    {
        if (navMeshAgent.pathPending) return false;
        if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) return false;
        return !navMeshAgent.hasPath || Math.Abs(navMeshAgent.velocity.sqrMagnitude) < 0.001f;
    }

    private void OnDrawGizmos()
    {
        if (navMeshAgent == null) return;
        
        Gizmos.color = Color.green;
        if (navMeshAgent.destination != default)
            Gizmos.DrawLine(navMeshAgent.destination, navMeshAgent.destination+(Vector3.up*4));
    }

    public void Feed()
    {
        energy -= 
    }
}
