using Mirror;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Health))]
public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private Unit unitPrefab;
    [SerializeField] private Transform unitSpawnLocation;
    [SerializeField] Health health;
    [SerializeField] int maxUnitQue = 5;
    [SerializeField] float spawnMoveRange = 15;
    [SerializeField] float unitSpawnDuration = 5;
    [SerializeField] SpawnQueUI spawnQueUI;

    [SyncVar(hook = nameof(ClientHandleQueuedUnitsUpdated))]
    private int queuedUnits;
    [SyncVar]
    private float unitTimer;

    private RTSPlayer rtsPlayer;

    private void Update()
    {
        if (isServer)
        {
            ProduceUnits();
        }
        if (isClient)
        {
            UpdateDisplay();
        }
    }





    #region Server
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
    }


    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;

    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        if (queuedUnits == maxUnitQue) { return; }
        if (rtsPlayer == null)
        {
            rtsPlayer = connectionToClient.identity.GetComponent<RTSPlayer>();
        }

        if (rtsPlayer.Resources < unitPrefab.Price) { return; }
        queuedUnits++;
        rtsPlayer.ModifyResources(-unitPrefab.Price);
    }

    [Server]
    private void ProduceUnits()
    {
        if (queuedUnits == 0) { return; }
        unitTimer += Time.deltaTime;
        if (unitTimer < unitSpawnDuration) { return; }

        GameObject newUnit = Instantiate(unitPrefab.gameObject, unitSpawnLocation.position, unitSpawnLocation.rotation);
        NetworkServer.Spawn(newUnit, connectionToClient);
        Vector3 spawnOffset = UnityEngine.Random.insideUnitSphere * spawnMoveRange;
        spawnOffset.y = unitSpawnLocation.position.y;
        newUnit.GetComponent<UnitMovement>().ServerMove(unitSpawnLocation.position + spawnOffset);
        queuedUnits--;
        unitTimer = 0;
    }
    #endregion

    #region Client
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        if (!isOwned) { return; }
        CmdSpawnUnit();
    }

    private void ClientHandleQueuedUnitsUpdated(int oldUnits, int newUnits)
    {
        spawnQueUI.SetRemainingUnits(newUnits);
    }

    private void UpdateDisplay()
    {
        if (queuedUnits == 0)
        {
            spawnQueUI.SetTimerValue(0);
        }
        else
        {
            spawnQueUI.SetTimerValue(unitTimer / unitSpawnDuration);
        }
    }
    #endregion
}
