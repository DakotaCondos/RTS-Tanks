using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Building))]
public class BuildingStats : MonoBehaviour
{
    public string buildingName;
    public string buildingDescription;
    public int buildingCost;
    public Sprite buildingSprite;
}
