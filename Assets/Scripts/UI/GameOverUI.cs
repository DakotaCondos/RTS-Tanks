using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextBlock winningPlayerTextBlock;
    [SerializeField] GameObject gameOverDisplayObject;
    void Start()
    {
        print("GameOverUi Start");
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
        gameOverDisplayObject.SetActive(false);

    }

    private void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;

    }
    private void ClientHandleGameOver(string winningPlayer)
    {
        print("GameOverUi ClientHandleGameOver");
        gameOverDisplayObject.SetActive(true);
        winningPlayerTextBlock.Text = winningPlayer;

    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
