using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new();
    [SerializeField] UnitSelectionHandler unitSelectionHandler;
    private Camera mainCamera;


    void Start()
    {
        mainCamera = Camera.main;

    }
    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }
        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
        foreach (Unit item in unitSelectionHandler.SelectedUnits)
        {
            item.UnitMovement.CmdMove(point);
        }
    }
}
