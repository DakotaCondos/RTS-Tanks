using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] GameObject unitBasePrefab;
    [SerializeField] GameObject gameOverHandlerPrefab;
    [SerializeField] Color player1;
    [SerializeField] Color player2;
    [SerializeField] Color player3;
    [SerializeField] Color player4;
    public List<RTSPlayer> rtsPlayers { get; } = new();

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        ClientOnDisconnected?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        RTSPlayer rtsPlayer = conn.identity.GetComponent<RTSPlayer>();
        rtsPlayer.SetTeamNumber(conn.connectionId + 1);
        Color teamColor;

        teamColor = conn.connectionId switch
        {
            0 => player1,
            1 => player2,
            2 => player3,
            3 => player4,
            _ => new Color(
                        UnityEngine.Random.Range(0f, 1f),
                        UnityEngine.Random.Range(0f, 1f),
                        UnityEngine.Random.Range(0f, 1f)),
        };

        rtsPlayer.SetTeamColor(teamColor);


        {
            Debug.LogWarning("Move This");
            //GameObject unitSpawner = Instantiate(unitBasePrefab, conn.identity.transform.position, conn.identity.transform.rotation);
            //NetworkServer.Spawn(unitSpawner, conn);
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        //Change This Later
        Debug.LogWarning("Dont forget to change how this works");
        if (SceneManager.GetActiveScene().name.StartsWith("Map"))
        {
            GameObject gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance);
        }
    }
}
