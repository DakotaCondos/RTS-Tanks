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
    RTSPlayer player;

    private void Awake()
    {
        currentHotbarItems = new();
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
}
