using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Targeting), typeof(Targetable), typeof(NavMeshAgent))]
public class UnitMovement : NetworkBehaviour
{
    NavMeshAgent navMeshAgent;
    Targeting targeting;
    Targetable targetable;
    private Camera mainCamera;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        targeting = GetComponent<Targeting>();
        targetable = GetComponent<Targetable>();
    }

    #region Server
    [ServerCallback]
    private void Update()
    {
        if (!navMeshAgent.hasPath) { return; }
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) { return; }
        navMeshAgent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        targeting.ClearTarget();

        //check if position is valid
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }
        navMeshAgent.SetDestination(hit.position);
    }



    #endregion

    #region Client

    #endregion

}
