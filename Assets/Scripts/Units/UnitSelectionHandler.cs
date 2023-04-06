using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new();
    [SerializeField] RectTransform SelectedAreaBox;
    private Camera mainCamera;
    public List<Unit> SelectedUnits { get; } = new();
    private RTSPlayer rtsPlayer;
    private Vector2 startPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }



    private void OnDestroy()
    {
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;

    }

    private void Update()
    {
        ActiveRTSPlayer();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void ActiveRTSPlayer()
    {
        if (rtsPlayer != null) { return; }
        if (NetworkClient.connection.identity.TryGetComponent<RTSPlayer>(out RTSPlayer player))
        {
            rtsPlayer = player;
        }
    }

    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit item in SelectedUnits)
            {
                item.Deselect();
            }
            SelectedUnits.Clear();
        }

        SelectedAreaBox.gameObject.SetActive(true);
        startPosition = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        SelectedAreaBox.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        SelectedAreaBox.anchoredPosition = startPosition + new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void ClearSelectionArea()
    {
        SelectedAreaBox.gameObject.SetActive(false);

        if (SelectedAreaBox.sizeDelta.magnitude == 0)
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

            return;
        }

        Vector2 min = SelectedAreaBox.anchoredPosition - (SelectedAreaBox.sizeDelta / 2);
        Vector2 max = SelectedAreaBox.anchoredPosition + (SelectedAreaBox.sizeDelta / 2);

        foreach (Unit item in rtsPlayer.PlayersUnits)
        {
            if (SelectedUnits.Contains(item)) { continue; }

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(item.transform.position);

            if (screenPosition.x > min.x && screenPosition.y > min.y && screenPosition.x < max.x && screenPosition.y < max.y)
            {
                SelectedUnits.Add(item);
                item.Select();
            }

        }

    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        SelectedUnits.Remove(unit);
    }
    private void ClientHandleGameOver(string winnerName)
    {
        enabled = false;
    }
}
