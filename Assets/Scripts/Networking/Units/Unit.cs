using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] UnityEvent onSelect;
    [SerializeField] UnityEvent onDeselect;


    #region
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
