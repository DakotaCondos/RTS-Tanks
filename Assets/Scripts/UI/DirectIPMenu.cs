using Mirror;
using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectIPMenu : MonoBehaviour
{
    [SerializeField] TextBlock ipInput;
    [SerializeField] TextBlock responseMessage;
    [SerializeField] Color errorMessageColor;
    [SerializeField] Color standardMessageColor;
    bool isInteractable = true;
    private bool shouldCycleString = true;

    private void Awake()
    {
        responseMessage.Text = "";
    }

    private void OnEnable()
    {
        RTSNetworkManager.ClientOnConnected += HandleClientConnected;
        RTSNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        RTSNetworkManager.ClientOnConnected -= HandleClientConnected;
        RTSNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void Join()
    {
        if (!isInteractable) { return; }

        responseMessage.Text = "";
        string input = ipInput.Text;

        if (!IsValidIPv4Address(input))
        {
            ErrorMessage("Invalid IP");
            return;
        }

        NetworkManager.singleton.networkAddress = input;
        NetworkManager.singleton.StartClient();
        isInteractable = false;
        StartCoroutine(ActivelyJoiningMessage());
    }

    public static bool IsValidIPv4Address(string ipAddress)
    {
        if (String.IsNullOrEmpty(ipAddress))
            return false;

        if (ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            return true;

        string[] octets = ipAddress.Split('.');
        if (octets.Length != 4)
            return false;

        foreach (string octet in octets)
        {
            if (!byte.TryParse(octet, out byte value))
                return false;

            if (value < 0 || value > 255)
                return false;
        }

        return true;
    }

    private void ErrorMessage(string message)
    {
        responseMessage.Text = message;
        responseMessage.Color = errorMessageColor;
    }
    private void StandardMessage(string message)
    {
        responseMessage.Text = message;
        responseMessage.Color = standardMessageColor;
    }

    private IEnumerator ActivelyJoiningMessage()
    {
        string connectingMessage = "Joining in progress.\nPlease wait";
        shouldCycleString = true;
        while (shouldCycleString)
        {
            string cycleString = "";
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.5f);
                if (!shouldCycleString) { break; }
                cycleString += ".";
                StandardMessage(connectingMessage + cycleString);
            }
        }
    }

    private void HandleClientConnected()
    {
        shouldCycleString = false;
        GetComponent<MainMenu>().ShowLobbyPanel();
    }

    private void HandleClientDisconnected()
    {
        //Debug.LogWarning($"{nameof(HandleClientDisconnected)} called from {name} is not yet implemented");
        StopCoroutine(ActivelyJoiningMessage());
        shouldCycleString = false;
        ErrorMessage("Could not connect");
        isInteractable = true;
    }
}
