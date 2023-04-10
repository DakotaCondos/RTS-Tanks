using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RTSPlayer : NetworkBehaviour
{
    private List<Unit> playersUnits = new();
    [SerializeField] private List<Building> playerBuildings = new();
    [SerializeField] private List<Building> allBuildableBuildings = new();
    [SerializeField] LayerMask buildingLayerMask = new();
    [SerializeField] float buildingRange = 8f;
    [SyncVar(hook = nameof(ClientHandleResourceChange))] int resources = 200;
    public event Action<int> ClientOnResourceChange;

    public List<Unit> PlayersUnits { get => playersUnits; }
    public List<Building> PlayerBuildings { get => playerBuildings; }
    public int Resources { get => resources; }

    private void Start()
    {
        DebugCheckForDuplicateBuildings();
    }

    public bool CanPlaceBuildingHere(BoxCollider buildingCollider, Vector3 point)
    {
        if (Physics.CheckBox(point + buildingCollider.center, buildingCollider.size / 2f, Quaternion.identity, buildingLayerMask))
        {
            return false;
        }

        foreach (var item in playerBuildings)
        {
            if ((point - item.transform.position).sqrMagnitude <= buildingRange * buildingRange)
            {
                return true;
            }
        }
        return false;
    }

    //Everything in dev should be removed on final build
    #region Dev
    private void DebugCheckForDuplicateBuildings()
    {
        // Check for duplicates in the list
        Debug.LogWarning("Checking for duplicate building id's");
        HashSet<int> ids = new HashSet<int>();
        foreach (Building building in allBuildableBuildings)
        {
            if (!ids.Add(building.Id))
            {
                Debug.LogWarning("Duplicate building id found: " + building.Id);
            }
        }
    }
    #endregion


    #region server
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
        Building.ServerOnBuildingSpawned += ServerHandleBuildingSpawned;
        Building.ServerOnBuildingDespawned += ServerHandleBuildingDespawned;
    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
        Building.ServerOnBuildingSpawned -= ServerHandleBuildingSpawned;
        Building.ServerOnBuildingDespawned -= ServerHandleBuildingDespawned;
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingId, Vector3 point, Quaternion rotation)
    {
        Building buildingToPlace = null;
        buildingToPlace = allBuildableBuildings.First(s => s.Id == buildingId);

        if (buildingToPlace == null)
        {
            Debug.LogWarning("buildingToPlace is null! Make sure you have added it to the Player prefab list of buildings");
            return;
        }

        if (resources < buildingToPlace.Price) { return; }

        BoxCollider buildingCollider = buildingToPlace.GetComponent<BoxCollider>();
        if (!CanPlaceBuildingHere(buildingCollider, point)) { return; }

        bool inRange = false;
        foreach (var item in playerBuildings)
        {
            if ((point - item.transform.position).sqrMagnitude <= buildingRange * buildingRange)
            {
                inRange = true;
                break;
            }
        }
        if (!inRange) { return; }

        GameObject buildingInstance = Instantiate(buildingToPlace.gameObject, point, rotation);
        NetworkServer.Spawn(buildingInstance, connectionToClient);
        ModifyResources(-buildingToPlace.Price);
    }

    [Server]
    public void ModifyResources(int difference)
    {
        resources += difference;
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

    private void ServerHandleBuildingSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        playerBuildings.Add(building);
    }

    private void ServerHandleBuildingDespawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        playerBuildings.Remove(building);
    }

    #endregion

    #region client

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { return; }
        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitDespawned;
        Building.AuthorityOnBuildingSpawned += AuthorityHandleBuildingSpawned;
        Building.AuthorityOnBuildingSpawned += AuthorityHandleBuildingDespawned;


    }
    public override void OnStopClient()
    {
        if (!isClientOnly || !isOwned) { return; }
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
        Building.AuthorityOnBuildingSpawned -= AuthorityHandleBuildingSpawned;
        Building.AuthorityOnBuildingSpawned -= AuthorityHandleBuildingDespawned;
    }

    private void AuthorityHandleBuildingSpawned(Building building)
    {
        playerBuildings.Add(building);
    }

    private void AuthorityHandleBuildingDespawned(Building building)
    {
        playerBuildings.Remove(building);
    }

    private void AuthorityHandleUnitSpawned(Unit unit)
    {
        playersUnits.Add(unit);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        playersUnits.Remove(unit);
    }

    private void ClientHandleResourceChange(int oldResources, int newResources)
    {
        ClientOnResourceChange?.Invoke(newResources);
    }






    #endregion

}
