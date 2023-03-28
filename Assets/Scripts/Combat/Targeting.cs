using Mirror;
using UnityEngine;

public class Targeting : NetworkBehaviour
{
    Targetable target;

    public Targetable Target { get => target; }
    public bool HasTarget { get { return target != null; } }

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
