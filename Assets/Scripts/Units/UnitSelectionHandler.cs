using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new();
    private Camera mainCamera;
    public List<Unit> SelectedUnits { get; } = new();
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (Unit item in SelectedUnits)
            {
                item.Deselect();
            }
            SelectedUnits.Clear();
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
        SelectedUnits.Add(unit);

        foreach (Unit item in SelectedUnits)
        {
            item.Select();
        }
    }
}
