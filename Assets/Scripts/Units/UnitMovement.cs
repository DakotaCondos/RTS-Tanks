using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Targeting), typeof(Targetable), typeof(NavMeshAgent))]
public class UnitMovement : NetworkBehaviour
{
    NavMeshAgent navMeshAgent;
    Targeting targeting;
    Targetable targetable;
    private Camera mainCamera;
    [SerializeField] float chaseRange;

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
        Targetable target = targeting.Target;
        if (target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)
            {
                navMeshAgent.SetDestination(target.transform.position);
            }
            else if (navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath();
            }
        }

        if (!navMeshAgent.hasPath) { return; }
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) { return; }
        navMeshAgent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        ServerMove(position);
    }

    [Server]
    public void ServerMove(Vector3 position)
    {
        targeting.ClearTarget();

        //check if position is valid
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }
        navMeshAgent.SetDestination(hit.position);
    }

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [Server]
    private void ServerHandleGameOver()
    {
        navMeshAgent.ResetPath();
    }

    #endregion

    #region Client

    #endregion

}
