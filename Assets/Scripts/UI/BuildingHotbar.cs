using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHotbar : MonoBehaviour
{
    [SerializeField] GameObject hotbarItemPrefab;
    [SerializeField] UIBlock2D hotbar;
    [SerializeField] List<BuildingHotbarItem> currentHotbarItems;
    RTSPlayer rtsPlayer = null;


    private void Awake()
    {
        currentHotbarItems = new();
    }

    private void Update()
    {
        ActiveRTSPlayer();
    }

    public void AddItemToHotbar(Building building)
    {
        foreach (var item in currentHotbarItems)
        {
            if (String.Equals(building.Name, item.BuildingName)) { return; }
        }
        GameObject hotbarItemInstance = Instantiate(hotbarItemPrefab, hotbar.transform);
        BuildingHotbarItem buildingHotbarItem = hotbarItemInstance.GetComponent<BuildingHotbarItem>();
        buildingHotbarItem.SetHotbarItemDetails(building);
        currentHotbarItems.Add(buildingHotbarItem);
    }

    public void ClearHotbarItems()
    {
        currentHotbarItems.Clear();
    }

    public void CalculateHotbarItems()
    {
        ClearHotbarItems();
        foreach (var item in rtsPlayer.PlayerBuildings)
        {
            if (!item.TryGetComponent<PlayerCanBuildThis>(out PlayerCanBuildThis unlocked)) { return; }

            AddItemToHotbar(unlocked.UnlockableBuilding);
        }
    }

    private void ActiveRTSPlayer()
    {
        if (rtsPlayer != null) { return; }
        if (NetworkClient.connection.identity.TryGetComponent<RTSPlayer>(out RTSPlayer player))
        {
            rtsPlayer = player;
        }

    }
}
