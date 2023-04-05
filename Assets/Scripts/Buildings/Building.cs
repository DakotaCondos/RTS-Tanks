using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : NetworkBehaviour
{
    [SerializeField] private Texture2D icon;
    [SerializeField] private int id = -1;
    [SerializeField] private int price;
    [SerializeField] private string buildingName;
    [SerializeField] private string buildingDescription;
    [SerializeField] private GameObject buildingBlueprint;

    public Texture2D Icon { get => icon; }
    public int Id { get => id; }
    public int Price { get => price; }
    public string Name { get => buildingName; }
    public string BuildingDescription { get => buildingDescription; }
    public GameObject BuildingBlueprint { get => buildingBlueprint; }

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
