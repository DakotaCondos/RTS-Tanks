using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] List<Unit> playersUnits = new();

    public List<Unit> PlayersUnits { get => playersUnits; }

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
    }

    #region server

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
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

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { return; }
        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitDespawned;
    }
    public override void OnStopClient()
    {
        if (!isClientOnly || !isOwned) { return; }
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;

    }
    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        playersUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        playersUnits.Remove(unit);
    }






    #endregion

}
