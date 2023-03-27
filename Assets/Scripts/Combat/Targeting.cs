using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : NetworkBehaviour
{
    [SerializeField] Targetable target;

    #region server
    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) { return; }
        this.target = target;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    #endregion

    #region client

    #endregion
}
