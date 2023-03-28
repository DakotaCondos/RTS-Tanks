using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Health))]
public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private List<GameObject> unitPrefabs = new();
    [SerializeField] private Transform unitSpawnLocation;
    [SerializeField] Health health;

    #region Server
    public override void OnStartServer()
    {
        health.ServerOnDie += HandleServerDie;
    }


    public override void OnStopServer()
    {
        health.ServerOnDie -= HandleServerDie;

    }

    [Server]
    private void HandleServerDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        //for now just grab a unit from the list
        GameObject newUnit = Instantiate(unitPrefabs.FirstOrDefault(), unitSpawnLocation.position, unitSpawnLocation.rotation);
        NetworkServer.Spawn(newUnit, connectionToClient);
    }
    #endregion

    #region Client
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        if (!isOwned) { return; }
        CmdSpawnUnit();
    }

    #endregion
}
