using Mirror;
using System;
using UnityEngine;

public class Targeting : NetworkBehaviour
{
    Targetable target;

    public Targetable Target { get => target; }
    public bool HasTarget { get { return target != null; } }

    #region server
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
