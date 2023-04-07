using Mirror;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnitMovement), typeof(Targeting), typeof(Targetable))]
[RequireComponent(typeof(Health))]
public class Unit : NetworkBehaviour
{
    [SerializeField] UnityEvent onSelect;
    [SerializeField] UnityEvent onDeselect;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] SpriteRenderer selectedSprite;
    [SerializeField] Targeting targeting;
    [SerializeField] Targetable targetable;
    [SerializeField] Health health;
    [SerializeField] int price = 20;

    public UnitMovement UnitMovement { get => unitMovement; }
    public Targeting Targeting { get => targeting; }
    public Targetable Targetable { get => targetable; }
    public int Price { get => price; }

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region server
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
        ServerOnUnitSpawned?.Invoke(this);
    }


    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        ServerOnUnitDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion


    #region client

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isOwned) { return; }
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
