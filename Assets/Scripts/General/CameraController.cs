using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using Nova;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Transform playerCameraTransform = null;
    [SerializeField] Color lineColor;
    [SerializeField] UIBlock2D minimapBoundsPrefab;
    [SerializeField] float boundsMultiplier = 1;
    public LayerMask floorLayer; // The layer mask for the floor objects

    public Vector3 focalPointMouse;
    public Vector3 focalPointScreenCenter;
    public float moveSpeed = 10f; // The speed at which the camera moves
    public float rotationSpeed = 100f; // The speed at which the camera rotates
    public float zoomSpeed = 10f; // The speed at which the camera zooms

    public float zoomDistance = 0f; // The initial distance between the camera and the focal point
    public float minZoom = 0f;
    public float maxZoom = 250f;
    public float minFov = 10f;
    public float maxFov = 120f;
    public CinemachineVirtualCamera virtualCamera;
    Camera mainCamera;

    Vector3 rotationPoint = Vector3.zero;
    Vector3 centerScreen = Vector3.zero;
    GameObject minimapBoundsInstance = null;
    UIBlock2D minimapBoundsUI = null;

    public override void OnStartAuthority()
    {
        playerCameraTransform.gameObject.SetActive(true);
        if (focalPointMouse == null)
        {
            focalPointMouse = Vector3.zero;
        }
        if (focalPointScreenCenter == null)
        {
            focalPointScreenCenter = Vector3.zero;
        }

        virtualCamera.LookAt = transform;
        virtualCamera.Follow = transform;
        mainCamera = Camera.main;

    }

    [ClientCallback]
    private void Update()
    {
        if (!isOwned || !Application.isFocused) { return; }
        if (mainCamera == null) { mainCamera = Camera.main; }
        UpdateCameraPosition();
        UpdateMinimapViewBounds();
    }

    private void UpdateMinimapViewBounds()
    {
        if (minimapBoundsInstance == null)
        {
            minimapBoundsInstance = Instantiate(minimapBoundsPrefab.gameObject);
            minimapBoundsUI = minimapBoundsInstance.GetComponent<UIBlock2D>();
        }

        centerScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        if (Physics.Raycast(mainCamera.ScreenPointToRay(centerScreen), out RaycastHit hit, 1500f, floorLayer))
        {
            focalPointScreenCenter = hit.point;
            minimapBoundsInstance.transform.position = hit.point;
            minimapBoundsUI.Size = new Length3((maxZoom - zoomDistance) * boundsMultiplier, (maxZoom - zoomDistance) * boundsMultiplier, 0);
        }

    }

    private void UpdateCameraPosition()
    {
        // Movement controls
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(moveSpeed * 3f * Time.deltaTime * new Vector3(horizontal, 0f, vertical));
        }
        else
        {
            transform.Translate(moveSpeed * Time.deltaTime * new Vector3(horizontal, 0f, vertical));

        }

        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1500f))
        {
            rotationPoint = hit.point;
        }


        // Rotation controls
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(rotationPoint, Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(rotationPoint, Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        // Zoom controls
        if (Input.GetKey(KeyCode.LeftShift)) { return; }
        float scroll = Input.mouseScrollDelta.y;
        float newZoomDistance = Mathf.Clamp(zoomDistance + scroll * zoomSpeed, minZoom, maxZoom);
        float zoomDelta = newZoomDistance - zoomDistance;
        zoomDistance = newZoomDistance;
        transform.position += zoomDelta * virtualCamera.transform.forward;
    }
}
