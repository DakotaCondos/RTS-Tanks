using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIBlock2D mainPanel;
    [SerializeField] UIBlock2D lobbyPanel;
    [SerializeField] UIBlock2D ipJoin;

    [SerializeField] List<UIBlock2D> panels;

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
    }

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
        NetworkManager.singleton.StartHost();
        //ShowLobbyPanel();
    }
}
