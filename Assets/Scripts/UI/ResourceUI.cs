using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] TextBlock resourceValue;

    private RTSPlayer rtsPlayer;

    private void Update()
    {
        ActiveRTSPlayer();
    }

    private void ActiveRTSPlayer()
    {
        if (rtsPlayer != null) { return; }
        try
        {
            if (NetworkClient.connection.identity.TryGetComponent<RTSPlayer>(out RTSPlayer player))
            {
                rtsPlayer = player;
                ClientHandleResourcesChanged(player.Resources);
                rtsPlayer.ClientOnResourceChange += ClientHandleResourcesChanged;
            }
        }
        catch (NullReferenceException)
        {
            return;
        }
    }

    private void ClientHandleResourcesChanged(int resources)
    {
        resourceValue.Text = resources.ToString();
    }

    private void OnDestroy()
    {
        rtsPlayer.ClientOnResourceChange -= ClientHandleResourcesChanged;
    }
}
