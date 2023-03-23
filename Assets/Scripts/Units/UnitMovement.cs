using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    private Camera mainCamera;

    #region Server
    [Command]
    public void CmdMove(Vector3 position)
    {
        //check if position is valid
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }
        navMeshAgent.SetDestination(hit.position);
    }



    #endregion

    #region Client

    #endregion

}
