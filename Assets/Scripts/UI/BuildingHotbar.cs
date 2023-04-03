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

    private void Awake()
    {
        currentHotbarItems = new();
    }

    public void AddItemToHotbar(BuildingStats stats)
    {
        foreach (var item in currentHotbarItems)
        {
            if (String.Equals(stats.buildingName, item.BuildingName)) { return; }
        }
        GameObject newHotbarItem = Instantiate(hotbarItemPrefab, hotbar.transform);
        currentHotbarItems.Add(newHotbarItem.GetComponent<BuildingHotbarItem>());
    }

    public void ClearHotbarItems()
    {
        currentHotbarItems.Clear();
    }
}
