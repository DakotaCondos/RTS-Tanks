using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] double maxHealth = 100;
    [SerializeField] private bool ownerlessEntity = false;

    [SyncVar]
    double currentHealth;

    public bool OwnerlessEntity { get => ownerlessEntity; }

    public event Action ServerOnDie;

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

    #endregion
}
