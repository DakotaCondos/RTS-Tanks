using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : NetworkBehaviour
{
    [SerializeField] Sprite icon;
    [SerializeField] int id;
    [SerializeField] int price;
    public BuildingStats stats;

    public Sprite Icon { get => icon; }
    public int Id { get => id; }
    public int Price { get => price; }


    public static event Action<Building> ServerOnBuildingSpawned;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnBuildingSpawned;
    public static event Action<Building> AuthorityOnBuildingDespawned;


    #region Server
    public override void OnStartServer()
    {
        ServerOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnBuildingDespawned?.Invoke(this);
    }
    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        AuthorityOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isOwned) { return; }
        AuthorityOnBuildingDespawned?.Invoke(this);
    }

    #endregion
}
