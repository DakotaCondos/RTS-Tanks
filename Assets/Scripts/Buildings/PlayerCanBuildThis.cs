using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanBuildThis : NetworkBehaviour
{
    [SerializeField] List<Building> unlockableBuildings;
    BuildingHotbar buildingHotbar;

    public List<Building> UnlockableBuildings { get => unlockableBuildings; }

    //the goal of this class is to allow only the player who placed a thing this script is attached to, to unlock new types of buildings to be placed
    private void Awake()
    {
        buildingHotbar = FindObjectOfType<BuildingHotbar>();
    }
    private void Start()
    {
        AddBuildingsToHotbar(UnlockableBuildings);
    }

    private void OnDestroy()
    {
        if (isOwned)
        {
            buildingHotbar.CalculateHotbarItems();
        }
    }

    private void AddBuildingsToHotbar(List<Building> buildings)
    {
        //if this is owned by clients connection to server
        if (isOwned)
        {
            foreach (var item in buildings)
            {
                buildingHotbar.AddItemToHotbar(item);
            }
        }
    }
}
