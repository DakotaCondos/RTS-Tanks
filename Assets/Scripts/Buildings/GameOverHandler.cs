using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameOverHandler : NetworkBehaviour
{
    private List<UnitBase> unitBases = new();

    #region Server

    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpawn += ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseSpawn += ServerHandleBaseDespawned;
    }

    public override void OnStopServer()
    {

        UnitBase.ServerOnBaseSpawn -= ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseSpawn -= ServerHandleBaseDespawned;
    }

    [Server]
    private void ServerHandleBaseSpawned(UnitBase unitBase)
    {
        unitBases.Add(unitBase);
    }

    private void ServerHandleBaseDespawned(UnitBase unitBase)
    {
        unitBases.Remove(unitBase);
        if (unitBases.Count != 1) { return; }

        //Game is over
        print($"GameOver! Player{unitBases.FirstOrDefault().netIdentity.connectionToClient} wins");
    }

    #endregion

    #region Client

    #endregion
}