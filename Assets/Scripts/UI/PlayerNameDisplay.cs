using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviour
{
    RTSPlayer rtsPlayer;
    [SerializeField] TextBlock playerNameTextBlock;

    void Start()
    {
        ActiveRTSPlayer();
        if (rtsPlayer == null)
        {
            Debug.LogWarning("Could not fint ActiveRTSPlayer");
            return;
        }
        playerNameTextBlock.Text = rtsPlayer.Displayname;
    }

    private void ActiveRTSPlayer()
    {
        if (rtsPlayer != null) { return; }
        try
        {
            if (NetworkClient.connection.identity.TryGetComponent<RTSPlayer>(out RTSPlayer player))
            {
                rtsPlayer = player;
            }
        }
        catch (NullReferenceException)
        {
            return;
        }
    }
}
