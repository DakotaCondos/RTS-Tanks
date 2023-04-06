using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanBuildThis : NetworkBehaviour
{
    [SerializeField] Building unlockableBuilding;
    BuildingHotbar buildingHotbar;

    public Building UnlockableBuilding { get => unlockableBuilding; }

    //the goal of this class is to allow only the player who placed a thing this script is attached to, to unlock new types of buildings to be placed
    private void Awake()
    {
        buildingHotbar = FindObjectOfType<BuildingHotbar>();
    }
    private void Start()
    {
        AddBuildingToHotbar(UnlockableBuilding);
    }

    private void OnDestroy()
    {
        if (isOwned)
        {
            buildingHotbar.CalculateHotbarItems();
        }
    }

    private void AddBuildingToHotbar(Building unlockableBuilding)
    {
        //if this is owned by clients connection to server
        if (isOwned)
        {
            buildingHotbar.AddItemToHotbar(unlockableBuilding);
        }
    }
}
