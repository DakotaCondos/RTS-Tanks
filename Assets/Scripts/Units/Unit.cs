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
    public UnitMovement UnitMovement { get => unitMovement; }

    public static event Action<Unit> ServerUnitSpawned;
    public static event Action<Unit> ServerUnitDespawned;

    public static event Action<Unit> AuthorityUnitSpawned;
    public static event Action<Unit> AuthorityUnitDespawned;

    #region server
    public override void OnStartServer()
    {
        ServerUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerUnitDespawned?.Invoke(this);
    }

    #endregion


    #region client

    public override void OnStartClient()
    {
        if (!isClientOnly || !isOwned) { return; }
        AuthorityUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !isOwned) { return; }
        AuthorityUnitDespawned?.Invoke(this);
    }



    [Client]
    public void Select()
    {
        if (!isOwned) return;
        onSelect?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!isOwned) return;
        onDeselect?.Invoke();
    }
    #endregion
}
