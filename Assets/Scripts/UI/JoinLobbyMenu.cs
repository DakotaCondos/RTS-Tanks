using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinLobbyMenu : MonoBehaviour
{
    private void OnEnable()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
    }
    private void OnDisable()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
    }

    private void HandleClientConnected()
    {
        Debug.LogWarning($"{nameof(HandleClientConnected)} called from {name} is not yet implemented");
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
