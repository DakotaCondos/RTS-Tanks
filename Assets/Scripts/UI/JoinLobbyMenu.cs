using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject startGameButton;
    [SerializeField] TextBlock[] playerNameTextBlocks;
    [SerializeField] bool showStartButton = false;
    [SerializeField] bool initialInfoCheckCompleted = false;
    private void OnEnable()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
        RTSPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyStateUpdated;
        RTSPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;

        startGameButton.SetActive(false);
    }



    private void OnDisable()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
        RTSPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyStateUpdated;
        RTSPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
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

    public void StartGame()
    {
        NetworkClient.connection.identity.GetComponent<RTSPlayer>().CmdStartGame();
    }

    private void HandleClientConnected()
    {
        Debug.LogWarning($"{nameof(HandleClientConnected)} called from {name} is not yet implemented");
    }

    private void AuthorityHandlePartyStateUpdated(bool value)
    {
        print($"Setting showStartButton = {value}");
        showStartButton = value;
        ClientHandleInfoUpdated();
    }

    private void ClientHandleInfoUpdated()
    {
        List<RTSPlayer> players = ((RTSNetworkManager)NetworkManager.singleton).RtsPlayers;
        print($"player count = {players.Count}");
        if (initialInfoCheckCompleted && players.Count == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }


        for (int i = 0; i < players.Count; i++)
        {
            playerNameTextBlocks[i].Text = players[i].Displayname;
        }
        for (int i = players.Count; i < playerNameTextBlocks.Length; i++)
        {
            playerNameTextBlocks[i].Text = "Waiting for player...";
        }

        startGameButton.SetActive(showStartButton && players.Count > 1);
        initialInfoCheckCompleted = true;

    }
}
