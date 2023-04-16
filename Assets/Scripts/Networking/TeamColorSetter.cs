using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorSetter : NetworkBehaviour
{
    [SerializeField] Renderer[] colorRenderers = new Renderer[0];
    [SerializeField] Material[] teamMaterials = new Material[0];
    [SerializeField] Renderer iconRenderer;

    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    Color teamColor = new();

    [SyncVar(hook = nameof(HandleTeamNumberUpdated))]
    int teamNumber = new();





    #region Server
    public override void OnStartServer()
    {
        RTSPlayer rtsPlayer = connectionToClient.identity.GetComponent<RTSPlayer>();
        teamColor = rtsPlayer.TeamColor;
        teamNumber = rtsPlayer.TeamNumber;
    }

    #endregion

    #region Client

    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        Material material = iconRenderer.material;
        material.color = newColor;
        float emissionIntensity = 2f;
        material.SetColor("_EmissionColor", newColor * emissionIntensity);
        if (teamNumber <= 3) { return; }

        foreach (var item in colorRenderers)
        {
            item.material.color = newColor;
        }
    }

    private void HandleTeamNumberUpdated(int oldNumber, int newNumber)
    {
        if (newNumber > 3) { return; }
        foreach (var item in colorRenderers)
        {
            item.material = teamMaterials[newNumber - 1];
        }
    }

    #endregion
}
