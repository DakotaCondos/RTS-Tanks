using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] List<Unit> playersUnits = new();

    public override void OnStartServer()
    {
        Unit.ServerUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerUnitDespawned += ServerHandleUnitDespawned;
    }

    #region server

    public override void OnStopServer()
    {
        Unit.ServerUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerUnitDespawned -= ServerHandleUnitDespawned;
    }
    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        playersUnits.Add(unit);
    }

    private void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        playersUnits.Remove(unit);
    }
    #endregion

    #region client

    public override void OnStartClient()
    {
        if (!isClientOnly) { return; }
        Unit.AuthorityUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityUnitSpawned += AuthorityHandleUnitDespawned;
    }
    public override void OnStopClient()
    {
        if (!isClientOnly) { return; }
        Unit.AuthorityUnitDespawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityUnitDespawned -= AuthorityHandleUnitDespawned;

    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        if (!isOwned) { return; }
        playersUnits.Add(unit);

    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        playersUnits.Remove(unit);

    }





    #endregion

}
