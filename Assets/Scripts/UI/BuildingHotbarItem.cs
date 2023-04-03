using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHotbarItem : MonoBehaviour
{
    [SerializeField] TextBlock buildingName;
    [SerializeField] UIBlock2D iconBlock;
    [SerializeField] TextBlock buildingPrice;

    public TextBlock BuildingName { get => buildingName; }

    public void SetHotbarItemDetails(string name, Texture2D icon, int price)
    {
        BuildingName.Text = name;
        iconBlock.SetImage(icon);
        buildingPrice.Text = price.ToString();
    }
}
