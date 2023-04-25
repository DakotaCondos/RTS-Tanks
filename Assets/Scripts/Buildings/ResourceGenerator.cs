using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : NetworkBehaviour
{
    [SerializeField] Health health;
    [SerializeField] int resourcesPerInterval = 20;
    [SerializeField] float secondsPerInterval = 5;

    private float timer = 0;
    RTSPlayer rtsPlayer;

    public override void OnStartServer()
    {
        rtsPlayer = connectionToClient.identity.GetComponent<RTSPlayer>();
        timer = secondsPerInterval;

        health.ServerOnDie += ServerHandleDie;
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    private void ServerHandleGameOver()
    {
        enabled = false;
    }

    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += secondsPerInterval;
            rtsPlayer.ModifyResources(resourcesPerInterval);
        }
    }
}
