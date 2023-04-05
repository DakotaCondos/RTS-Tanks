using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHotbarItem : MonoBehaviour
{
    [SerializeField] TextBlock buildingName;
    [SerializeField] UIBlock2D iconBlock;
    [SerializeField] TextBlock buildingPrice;
    [SerializeField] LayerMask floorLayerMask = new();
    [SerializeField] GameObject buildingPrefab;
    [SerializeField] GameObject buildingBlueprint;
    private Camera mainCamera;
    private RTSPlayer rtsPlayer = null;
    private GameObject buildingPreviewInstance = null;
    private bool isBuildingPrefab = false;
    public TextBlock BuildingName { get => buildingName; }

    [SerializeField] float rotationSpeed = 10f;

    private void Awake()
    {
        mainCamera = Camera.main;
    }



    private void Update()
    {
        if (rtsPlayer == null)
        {
            rtsPlayer = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        Placement();
        BlueprintRotation();
    }

    private void BlueprintRotation()
    {
        if (!isBuildingPrefab || buildingPreviewInstance == null) { return; }
        buildingPreviewInstance.transform.Rotate(new Vector3(0, Input.mouseScrollDelta.y * rotationSpeed, 0));
    }

    private void Placement()
    {
        if (!isBuildingPrefab) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, floorLayerMask))
            {
                if (IsPlacementValid(hit))
                {
                    PlaceBuilding(hit);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RemovePreviewBuilding();
        }
        if (isBuildingPrefab)
        {
            UpdatePreviewBuildingPosition();
        }
    }


    public void SetHotbarItemDetails(Building building)
    {
        BuildingName.Text = building.Name;
        iconBlock.SetImage(building.Icon);
        buildingPrice.Text = building.Price.ToString();
        buildingBlueprint = building.BuildingBlueprint;
        buildingPrefab = building.gameObject;
    }

    public void StartBuildingPlacement()
    {
        isBuildingPrefab = true;
        Quaternion cameraRotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        buildingPreviewInstance = Instantiate(buildingBlueprint, Vector3.zero, cameraRotation);
    }

    private void UpdatePreviewBuildingPosition()
    {
        RaycastHit hit;
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, floorLayerMask)) { return; }
        buildingPreviewInstance.transform.position = hit.point;
    }

    private void PlaceBuilding(RaycastHit hit)
    {
        rtsPlayer.CmdTryPlaceBuilding(buildingPrefab.GetComponent<Building>().Id, hit.point, buildingPreviewInstance.transform.rotation);
        RemovePreviewBuilding();
    }

    private void RemovePreviewBuilding()
    {
        Destroy(buildingPreviewInstance);
        isBuildingPrefab = false;
        buildingPreviewInstance = null;
    }

    private bool IsPlacementValid(RaycastHit hit)
    {
        // TODO: Implement server placement validation logic here
        return true;
    }
}
