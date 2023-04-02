using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthDisplay))]
public class Health : NetworkBehaviour
{
    [SerializeField] private bool isOwnerlessEntity = false;
    [SerializeField] double maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthChange))] double currentHealth;

    public event Action<double, double> ClientOnHealthChange;
    public event Action ServerOnDie;

    public bool OwnerlessEntity { get => isOwnerlessEntity; }

    #region Server
    public override void OnStartServer()
    {
        currentHealth = maxHealth;
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionId)
    {
        if (isOwnerlessEntity) { return; }

        if (connectionToClient.connectionId != connectionId) { return; }
        DealDamage(currentHealth);
    }


    [Server]
    public void DealDamage(double damageAmount)
    {
        if (currentHealth == 0) { return; }
        currentHealth -= damageAmount;
        currentHealth = (currentHealth < 0) ? 0 : currentHealth;
        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();
    }

    #endregion

    #region Client
    private void HandleHealthChange(double oldHealth, double newHealth)
    {
        ClientOnHealthChange?.Invoke(newHealth, maxHealth);
    }

    #endregion
}
