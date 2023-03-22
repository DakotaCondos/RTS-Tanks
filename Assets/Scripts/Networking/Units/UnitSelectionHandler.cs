using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new();
    private Camera mainCamera;
    private List<Unit> selectedUnits = new();
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (Unit item in selectedUnits)
            {
                item.Deselect();
            }
            selectedUnits.Clear();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }
        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }
        if (!unit.isOwned) { return; }
        selectedUnits.Add(unit);

        foreach (Unit item in selectedUnits)
        {
            item.Select();
        }
    }
}
