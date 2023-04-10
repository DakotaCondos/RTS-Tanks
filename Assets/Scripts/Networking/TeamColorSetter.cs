using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorSetter : NetworkBehaviour
{
    [SerializeField] Renderer[] colorRenderers = new Renderer[0];

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    Color teamColor = new();



    #region Server
    public override void OnStartServer()
    {
        RTSPlayer rtsPlayer = connectionToClient.identity.GetComponent<RTSPlayer>();
        teamColor = rtsPlayer.TeamColor;
    }

    #endregion

    #region Client

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach (var item in colorRenderers)
        {
            item.material.color = newColor;
        }
    }
    #endregion
}
