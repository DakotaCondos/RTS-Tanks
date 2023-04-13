using Mirror;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Targeting : NetworkBehaviour
{
    Targetable target;

    public Targetable Target { get => target; }
    public bool HasTarget { get { return target != null; } }

    [Header("Turret Settings")]
    [Tooltip("Only add an object you wish to rotate using the turret settings below")]
    [SerializeField] GameObject turret = null;
    [Tooltip("If the turret object is rotated enter that as an offset here")]
    [SerializeField] float rotationX = 0;
    [Tooltip("If the turret object is rotated enter that as an offset here")]
    [SerializeField] float rotationY = 0;
    [Tooltip("If the turret object is rotated enter that as an offset here")]
    [SerializeField] float rotationZ = 0;

    #region server
    [Server]
    private void Update()
    {
        PointTurretAtTarget();
    }

    [Server]
    private void PointTurretAtTarget()
    {
        if (turret == null) { return; }
        if (target == null) { return; }

        Vector3 directionToTarget = target.transform.position - turret.transform.position;
        directionToTarget.y = 0; //Prevent Y axis movement
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        Vector3 euler = targetRotation.eulerAngles;
        turret.transform.rotation = Quaternion.Euler(Vector3.zero);
        turret.transform.Rotate(euler.x + rotationX, euler.y + rotationY, euler.z + rotationZ, Space.World);
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) { return; }
        this.target = target;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
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
        ClearTarget();
    }
    #endregion

    #region client


    #endregion
}
