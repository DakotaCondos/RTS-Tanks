using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Transform playerCameraTransform = null;

    public Transform focalPoint; // The object to rotate around
    public float moveSpeed = 10f; // The speed at which the camera moves
    public float rotationSpeed = 100f; // The speed at which the camera rotates
    public float zoomSpeed = 10f; // The speed at which the camera zooms

    public float zoomDistance = 95f; // The initial distance between the camera and the focal point
    public float minZoom = 0f;
    public float maxZoom = 100f;
    public float minFov = 10f;
    public float maxFov = 120f;
    public CinemachineVirtualCamera virtualCamera;
    Camera mainCamera;

    public override void OnStartAuthority()
    {
        playerCameraTransform.gameObject.SetActive(true);
        if (focalPoint == null)
        {
            Debug.LogError("No focal point assigned to CameraController!");
            return;
        }

        virtualCamera.LookAt = focalPoint;
        virtualCamera.Follow = transform;
        mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (!isOwned || !Application.isFocused) { return; }

        UpdateCameraPosition();
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

        Vector3 rotationPoint = focalPoint.position;

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
