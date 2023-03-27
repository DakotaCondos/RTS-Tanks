using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] UnityEvent onSelect;
    [SerializeField] UnityEvent onDeselect;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] SpriteRenderer selectedSprite;

    public UnitMovement UnitMovement { get => unitMovement; }

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region server
    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion


    #region client

    public override void OnStartClient()
    {
        if (!isClientOnly || !isOwned) { return; }
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !isOwned) { return; }
        AuthorityOnUnitDespawned?.Invoke(this);
    }



    [Client]
    public void Select()
    {
        if (!isOwned) return;
        onSelect?.Invoke();
        //show selected sprite
        selectedSprite.color = Color.green;
    }

    [Client]
    public void Deselect()
    {
        if (!isOwned) return;
        onDeselect?.Invoke();
        //hide selected sprite
        selectedSprite.color = Color.clear;
    }
    #endregion
}
