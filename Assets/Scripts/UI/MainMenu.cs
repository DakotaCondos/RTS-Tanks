using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using Unity.VisualScripting;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIBlock2D mainPanel;
    [SerializeField] UIBlock2D lobbyPanel;
    [SerializeField] UIBlock2D ipJoin;

    [SerializeField] List<UIBlock2D> panels;

    [SerializeField] bool useSteam = false;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;


    private void Awake()
    {
        panels = new List<UIBlock2D>
        {
            lobbyPanel,
            mainPanel,
            ipJoin
        };
    }

    private void Start()
    {
        ShowMainPanel();

        if (!useSteam) { return; }
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

    }

    #region Steam
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            ShowLobbyPanel();
            return;
        }

        NetworkManager.singleton.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress",
            SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress");

        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();

        ShowLobbyPanel();
    }
    #endregion


    public void ShowLobbyPanel()
    {
        SwitchPanel(lobbyPanel);
    }
    public void ShowMainPanel()
    {
        SwitchPanel(mainPanel);
    }
    public void ShowIPJoinPanel()
    {
        SwitchPanel(ipJoin);
    }

    public void SwitchPanel(UIBlock2D panel)
    {
        foreach (var item in panels)
        {
            item.gameObject.SetActive(item == panel);
        }
    }

    public void HostLobby()
    {
        if (useSteam)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
            return;
        }
        NetworkManager.singleton.StartHost();
    }
}
