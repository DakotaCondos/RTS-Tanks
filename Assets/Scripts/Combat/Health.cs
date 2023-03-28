using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthDisplay))]
public class Health : NetworkBehaviour
{
    [SerializeField] private bool ownerlessEntity = false;
    [SerializeField] double maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthChange))] double currentHealth;

    public event Action<double, double> ClientOnHealthChange;
    public event Action ServerOnDie;

    public bool OwnerlessEntity { get => ownerlessEntity; }

    #region Server
    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(double damageAmount)
    {
        if (currentHealth == 0) { return; }
        currentHealth -= damageAmount;
        currentHealth = (currentHealth < 0) ? 0 : currentHealth;
        if (currentHealth != 0) { return; }

        // die
        print($"{name} Died");
    }

    #endregion

    #region Client
    private void HandleHealthChange(double oldHealth, double newHealth)
    {
        ClientOnHealthChange?.Invoke(newHealth, maxHealth);
    }

    #endregion
}
